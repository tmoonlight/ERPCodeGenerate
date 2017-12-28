using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using EnvDTE80;
using CY_System.CodeBuilder;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using CodeBuilder;
using System.Text;
using System.Net;

namespace VSIXProject4
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Command1
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("981cc1f8-9093-4456-965b-11c1a3873e2b");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command1"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private Command1(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Command1 Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new Command1(package);
        }

        public const string ContentRootPath = @"C:\CYSystemCodeGenerator\";



        LoginForm SingleLoginForm = null;
        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            DTE dte = (DTE)ServiceProvider.GetService(typeof(DTE));
            CommonSettings.VSProjects.Clear();
            CommonSettings.TProjects.Clear();
            //var folderStr = dte.Solution.Properties.Settings.Default.SF_GUID;
            //初始化必要的变量

            foreach (EnvDTE.Project proj in dte.Solution.Projects)
            {
                if (proj.Kind != "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}" && proj.Kind != "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                {
                    //如遇文件夹则遍历一次(为减少复杂度,只遍历一层深度)
                    CommonSettings.VSProjects.Add(proj.Name, proj.Name);
                }
                else
                {

                    //文件夹,对文件夹再一次遍历获取某文件夹下所有的子文件,并且拼接路径
                    SetAllProjectFromFolder(proj);
                }
            }


            DirectoryInfo di = new DirectoryInfo(ContentRootPath + "CodeTemplate");
            foreach (var dir in di.GetDirectories())
            {
                CommonSettings.TProjects.Add(dir.Name, dir.Name);
            }
            CommonSettings.CurrProjectDir = Path.GetDirectoryName(dte.Solution.FullName);

            //传递项目文件,上次登录配置到这个界面
            if (SingleLoginForm == null || SingleLoginForm.IsDisposed)
            {
                SingleLoginForm = new LoginForm();
                //点击生成按钮时触发
                SingleLoginForm.Generate += Lf_Generate;
            }
            SingleLoginForm.Show();

        }

        private void SetAllProjectFromFolder(Project proj)
        {
            foreach (ProjectItem p in proj.ProjectItems)
            {//如果projectitem为空,则说明该元素是一个"项目",否则依然还是一个文件夹,则需要继续遍历(为避免复杂度,只支持一层)
                if (p.SubProject != null && p.ProjectItems == null && p.Kind != "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}")
                {
                    CommonSettings.VSProjects.Add(proj.Name + "\\" + p.Name, p.Name);
                }
            }
        }

        /// <summary>
        /// 通过选到的表和模板项目对应关系来生成代码
        /// </summary>
        /// <param name="TableNames">表名</param>
        /// <param name="dict">模板-项目对应</param>
        /// <returns></returns>
        private async Task<bool> Lf_Generate(List<Tuple<string, string, string>> TableNames, Dictionary<string, string> dict, IProgress<int> progress)
        {
            string templateFilePath = System.IO.Path.GetFullPath(ContentRootPath + "CodeTemplate");
            string bakPath = System.IO.Path.GetFullPath(ContentRootPath + "CodeGen_" + DateTime.Now.ToString("yyyyMMddhhmmss"));

            progress.Report(1);
            //1.复制文件模板,并套用模板到备份地址
            await System.Threading.Tasks.Task.Run(new Action(() =>
            {
                foreach (var tbname in TableNames)
                {
                    GenerateCodesInDir(templateFilePath, bakPath, tbname);
                }
            }));
            progress.Report(2);


            foreach (var fulldir in Directory.GetDirectories(bakPath))
            {
                var dirname = Path.GetFileName(fulldir);
                if (dict.ContainsKey(dirname))
                {

                    var files = Directory.GetFiles(fulldir, "*.*", SearchOption.AllDirectories);
                    var fileList = files.ToList<string>();
                    //2.将文件从备份地址加入项目
                    await System.Threading.Tasks.Task.Run(new Action(() => { EnvDTEHelper.AddFilesToProject(dict[dirname], fileList, bakPath + "\\" + dirname, this.ServiceProvider); }));
                }
            }
            progress.Report(3);

            return true;
        }



        /// <summary>
        /// 将某个表相关的整个模板加入参数进行生成
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <param name="TableName"></param>
        public static void GenerateCodesInDir(string SourcePath, string DestinationPath, Tuple<string, string, string> t3)
        {
            string tableName = t3.Item1;
            string tableConnectionString = t3.Item2;
            string tableModel = t3.Item3;
            string tableDescription = t3.Item3;
            DataTable tableInfo = SQLServerDBHelper.GetTableInfo(tableConnectionString, tableName);
            //string 
            string fieldCode = GenerateFieldCode(tableInfo);
            //创建所有文件夹
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //套用模板并且复制文件到新的备份目录
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
            {
                //文件模板套用
                string content = File.ReadAllText(newPath);
                content = content.Replace("##Author##", Dns.GetHostName())
                    .Replace("##DateTime##", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .Replace("##TableDescription##", tableModel)
                    .Replace("##ModelName##", tableModel)
                    .Replace("##Fields##", fieldCode)
                    .Replace("##TableName##", tableName);
                //替换文件名
                File.WriteAllText(newPath.Replace(SourcePath, DestinationPath).Replace("Order", tableModel)
                        .Replace(".template", ".cs"), content);
            }
        }

        /// <summary>
        /// 按表来生成poco实体列表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string GenerateFieldCode(DataTable table)
        {
            StringBuilder sbMethod = new StringBuilder();
            foreach (DataRow _DataRow in table.Rows)
            {
                string _colomnType = CommonMaker.GetType(_DataRow["TypeName"].ToString());
                _colomnType = _colomnType != "string" ? _colomnType + "?" : _colomnType;
                string _colomnName = _DataRow["ColumnName"].ToString();
                string _colomnNameUpper = _colomnName;
                string _colomnNameLower = _colomnName.ToLower();

                //sbProperty.AppendLine("        protected " + _colomnType + " m_" + _colomnNameLower + ";");

                sbMethod.AppendLine("        /// <summary>");
                sbMethod.AppendLine(string.Format("        /// {0}", _DataRow["deText"].ToString()==""? _DataRow["ColumnName"]+"属性" : _DataRow["deText"].ToString()));
                sbMethod.AppendLine("        /// <summary>");
                //sbMethod.AppendLine("        [DataMember]");
                //sbMethod.Append("        [EntityField(");

                //if (_DataRow["isPK"].ToString() == "√")
                //{
                //    sbMethod.Append("IsKey=true, ");
                //}
                //sbMethod.Append("DBType=System.Data.SqlDbType." + CommonMaker.GetEntityFieldTypeAndSize(_DataRow));
                //sbMethod.AppendLine(")]");

                sbMethod.AppendLine("        public " + _colomnType + " " + _colomnNameUpper + " { get; set; }");
                sbMethod.AppendLine();

            }


            //builder.AppendLine("    }");
            //builder.AppendLine("}");
            //foreach
            return sbMethod.ToString();
        }
    }
}
