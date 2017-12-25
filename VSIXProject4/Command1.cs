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
            //初始化必要的变量
            foreach (EnvDTE.Project proj in dte.Solution.Projects)
            {
                CommonSettings.VSProjects.Add(proj.Name, proj.Name);
            }

            DirectoryInfo di = new DirectoryInfo("CodeTemplate");
            foreach (var dir in di.GetDirectories())
            {
                CommonSettings.TProjects.Add(dir.Name, dir.Name);
            }
            CommonSettings.CurrProjectDir = Path.GetDirectoryName(dte.Solution.FullName);

            //传递项目文件,上次登录配置到这个界面
            LoginForm lf = new LoginForm();
            //点击生成按钮时触发
            lf.Generate += Lf_Generate;
            lf.Show();

        }

        /// <summary>
        /// 通过选到的表和模板项目对应关系来生成代码
        /// </summary>
        /// <param name="TableNames">表名</param>
        /// <param name="dict">模板-项目对应</param>
        /// <returns></returns>
        private bool Lf_Generate(List<Tuple<string, string>> TableNames, Dictionary<string, string> dict)
        {
            string templateFilePath = System.IO.Path.GetFullPath("CodeTemplate");
            string bakPath = System.IO.Path.GetFullPath("CodeGen_" + DateTime.Now.ToString("yyyyMMddhhmmss"));
            //1.复制文件模板
            foreach (var tbname in TableNames)
            {
                CopyDir(templateFilePath, bakPath, tbname.Item1);
            }

            foreach (var fulldir in Directory.GetDirectories(bakPath))
            {
                var dirname = Path.GetFileName(fulldir);
                if (dict.ContainsKey(dirname))
                {
                    //2.套用文件
                    var files = Directory.GetFiles(fulldir, "*.*", SearchOption.AllDirectories);
                    var fileList = files.ToList<string>();
                    //3.加入项目
                    EnvDTEHelper.AddFilesToProject(dict[dirname], fileList, bakPath + "\\" + dirname, this.ServiceProvider);
                }
            }

            return true;
        }



        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <param name="TableName"></param>
        public static void CopyDir(string SourcePath, string DestinationPath, string TableName)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
            {
                //文件模板应用
                string content = File.ReadAllText(newPath);
                File.WriteAllText(newPath.Replace(SourcePath, DestinationPath).Replace("Order", TableName)
                        .Replace(".template", ".cs"), content);
            }
        }


    }
}
