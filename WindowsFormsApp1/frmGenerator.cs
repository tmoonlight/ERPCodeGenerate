using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CY_System.CodeBuilder
{
    public partial class frmGenerator : Form
    {
        public frmGenerator()
        {
            InitializeComponent();

        }

        private void btnChangeDbConn_Click(object sender, EventArgs e)
        {
            if (new LoginForm(true).ShowDialog() == DialogResult.OK)
            {
                trvDBInfo.SelectedNode.Remove();
                trvDBInfo.Nodes.Clear();
                LoadData();
            }
        }

        private void frmGenerator_Load(object sender, EventArgs e)
        {
            //1.查询出当前连接所有数据库
            //_DataBaseList = SQLServerDBHelper.GetDataBase(DBSettings._conString.ToString());

            tbxSolutionDir.Text = CommonSettings.CurrProjectDir;

            LoadData();

            BindDDL();

            //初始化控件的一些值
            InitControls();
        }

        private void InitControls()
        {
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripStatusLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel2.Text = "";
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        }

        public Dictionary<string, string> GetTemplate2ProjectDict()
        {
            Dictionary<string, string> Template2Project = new Dictionary<string, string>();
            if (!DDLIsNull(ddlBLL))
            {
                Template2Project.Add("CY_System.BLL", ddlBLL.SelectedItem.ToString());
            }
            if (!DDLIsNull(ddlDomain))
            {
                Template2Project.Add("CY_System.DomainStandard", ddlDomain.SelectedItem.ToString());
            }
            if (!DDLIsNull(ddlRepository))
            {
                Template2Project.Add("CY_System.Infrastructure", ddlRepository.SelectedItem.ToString());
            }
            if (!DDLIsNull(ddlService))
            {
                Template2Project.Add("CY_System.Service", ddlService.SelectedItem.ToString());
            }
            if (!DDLIsNull(ddlDTO))
            {
                Template2Project.Add("CY_System.Service.Dto.Shared", ddlDTO.SelectedItem.ToString());
            }
            return Template2Project;
        }

        public bool DDLIsNull(ComboBox cbx)
        {
            return cbx.SelectedItem == null || cbx.SelectedItem.ToString() == "";
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDDL()
        {
            BindDicToDDL(ddlBLL, CommonSettings.VSProjects);
            BindDicToDDL(ddlDomain, CommonSettings.VSProjects);
            BindDicToDDL(ddlDTO, CommonSettings.VSProjects);
            BindDicToDDL(ddlRepository, CommonSettings.VSProjects);

            BindDicToDDL(ddlService, CommonSettings.VSProjects);


            if (CommonSettings.VSProjects.ContainsKey("CY_System.BLL"))
            {
                ddlBLL.SelectedItem = "CY_System.BLL";
            }
            if (CommonSettings.VSProjects.ContainsKey("CY_System.DomainStandard"))
            {
                ddlDomain.SelectedItem = "CY_System.DomainStandard";
            }
            if (CommonSettings.VSProjects.ContainsKey("CY_System.Infrastructure"))
            {
                ddlRepository.SelectedItem = "CY_System.Infrastructure";
            }
            if (CommonSettings.VSProjects.ContainsKey("CY_System.Service"))
            {
                ddlService.SelectedItem = "CY_System.Service";
            }
            if (CommonSettings.VSProjects.ContainsKey("CY_System.Service.Dto.Shared"))
            {
                ddlDTO.SelectedItem = "CY_System.Service.Dto.Shared";
            }

        }

        public void BindDicToDDL(ComboBox ddl, Dictionary<string, string> dict)
        {
            ddl.Items.Clear();
            foreach (var kv in dict)
            {
                ddl.Items.Add(kv.Key);
            }
            //ddlBLL.DataSource = CommonSettings.VSProjects;
            //foreach(key)
        }

        /// <summary>
        /// 加载
        /// </summary>
        public void LoadData()
        {

            TreeNode tn = new TreeNode("已经连接的数据库：");
            tn.Tag = "Root";
            tn.ImageIndex = 1;
            trvDBInfo.Nodes.Add(tn);
            foreach (DBConfig _DBConfig in DBSettings.DataBaseConfigList)
            {
                if (trvDBInfo.Nodes[0].Nodes.Count > 0)
                {
                    if (isNewServer(_DBConfig, trvDBInfo.Nodes[0]))
                    {
                        TreeNode tnServer = new TreeNode(_DBConfig.ServerName);
                        tnServer.Tag = "ServerName";
                        tnServer.ImageIndex = 0;

                        tnServer.Nodes.Add(LoadDataBase(_DBConfig));
                        trvDBInfo.Nodes[0].Nodes.Add(tnServer);
                    }
                    else
                    {
                        foreach (TreeNode _tnServer in trvDBInfo.Nodes[0].Nodes)
                        {
                            if (_tnServer.Text == _DBConfig.ServerName)
                            {
                                if (isNewDataBase(_DBConfig, _tnServer))
                                {
                                    _tnServer.Nodes.Add(LoadDataBase(_DBConfig));
                                }
                            }
                        }
                    }
                }
                else
                {
                    TreeNode tnServer = new TreeNode(_DBConfig.ServerName);
                    tnServer.Tag = "ServerName";
                    tnServer.ImageIndex = 0;

                    tnServer.Nodes.Add(LoadDataBase(_DBConfig));
                    trvDBInfo.Nodes[0].Nodes.Add(tnServer);
                }
            }
        }

        public TreeNode LoadDataBase(DBConfig _DBConfig)
        {
            TreeNode tn = new TreeNode();
            tn.Text = _DBConfig.DataBase;
            tn.ImageIndex = 1;
            tn.Tag = "DataBase";
            //tn.ContextMenuStrip = cmsForDataBase;

            TreeNode tnDataTables = new TreeNode();
            tnDataTables.Text = "数据表";
            tnDataTables.ImageIndex = 2;
            tnDataTables.Tag = "DataTables";

            DataTable dtTables = new DataTable();
            dtTables = SQLServerDBHelper.GetTables(_DBConfig.ConString, "U");
            for (int i = 0; i < dtTables.Rows.Count; i++)
            {
                DataRow tableRow = dtTables.Rows[i];
                TreeNode tnTable = new TreeNode();
                tnTable.Text = tableRow[0].ToString();
                tnTable.ImageIndex = 3;
                tnTable.Tag = "Table";
                //tnTable.ContextMenuStrip = cmsForTable;
                tnDataTables.Nodes.Add(tnTable);
            }

            TreeNode tnDataView = new TreeNode("视图");
            //tnDataView.Nodes[0].show
            tnDataView.ImageIndex = 2;
            tnDataView.Tag = "DataViews";
            //tnDataView.

            DataTable dtViews = new DataTable();
            dtViews = SQLServerDBHelper.GetTables(_DBConfig.ConString, "V");
            for (int j = 0; j < dtViews.Rows.Count; j++)
            {
                DataRow viewRow = dtViews.Rows[j];
                TreeNode tnView = new TreeNode();
                tnView.Text = viewRow[0].ToString();
                tnView.ImageIndex = 4;
                tnView.Tag = "View";
                //tnView.ContextMenuStrip = cmsForView;
                tnDataView.Nodes.Add(tnView);
                //HideCheckBox(tnView);
            }

            TreeNode tnDataProc = new TreeNode("存储过程");
            tnDataProc.ImageIndex = 2;
            tnDataProc.Tag = "DataProcs";

            DataTable dtProcs = new DataTable();
            dtProcs = SQLServerDBHelper.GetTables(_DBConfig.ConString, "P");
            for (int k = 0; k < dtProcs.Rows.Count; k++)
            {
                DataRow procRow = dtProcs.Rows[k];
                TreeNode tnProc = new TreeNode();
                tnProc.Text = procRow[0].ToString();
                tnProc.ImageIndex = 5;
                tnProc.Tag = "Proc";
                //tnProc.ContextMenuStrip = cmsForProc;
                tnDataProc.Nodes.Add(tnProc);
                //HideCheckBox(tnProc);
            }

            tn.Nodes.Add(tnDataTables);
            tn.Nodes.Add(tnDataView);
            tn.Nodes.Add(tnDataProc);
            return tn;
        }

        internal bool isNewServer(DBConfig _DBConfig, TreeNode _ServerFather)
        {
            foreach (TreeNode _tnServer in _ServerFather.Nodes)
            {
                if (_DBConfig.ServerName == _tnServer.Text)
                {
                    return false;
                }
            }
            return true;
        }

        internal bool isNewDataBase(DBConfig _DBConfig, TreeNode _DataBaseFather)
        {
            foreach (TreeNode _tnDataBase in _DataBaseFather.Nodes)
            {
                if (_tnDataBase.Text == _DBConfig.DataBase)
                {
                    return false;
                }
            }
            return true;
        }

        private void trvDBInfo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex;

        }

        private void cbxBLL_CheckedChanged(object sender, EventArgs e)
        {
            var cbx = (sender as CheckBox);
            switch (cbx.Tag)
            {
                case "BLL": ddlBLL.Enabled = cbx.Checked; break;
                case "Domain": ddlDomain.Enabled = cbx.Checked; break;
                case "DTO": ddlDTO.Enabled = cbx.Checked; break;
                case "Repository": ddlRepository.Enabled = cbx.Checked; break;
                case "Service": ddlService.Enabled = cbx.Checked; break;
                    // default ;
            }

            //.Checked = ddlBLL.Enabled;

        }

        public event GenerateHandler Generate;

        public delegate Task<bool> GenerateHandler(List<Tuple<string, string>> tableNames, Dictionary<string, string> projects, IProgress<int> progress);
        private async void button1_Click(object sender, EventArgs e)
        {
            btnGenterate.Enabled = false;
            btnClose.Enabled = false;
            //进度获取
            toolStripProgressBar1.Maximum = 3;// tableNames.Count;
            Progress<int> progress = new Progress<int>();
            progress.ProgressChanged += (sd, intValue) => {
                toolStripProgressBar1.Value = intValue;
            };
            await GenerateCodeFiles(progress);
            btnGenterate.Enabled = true;
            btnClose.Enabled = true;
        }

        private async Task GenerateCodeFiles(IProgress<int> progInt)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            toolStripStatusLabel2.Text = "生成中...";
            frmLoading loadingBox = new frmLoading();
            loadingBox.TopMost = true;
            loadingBox.StartPosition = FormStartPosition.CenterScreen;
            loadingBox.Show();
            Application.DoEvents();
            //1表名 2连接字符串
            List<Tuple<string, string>> list = new List<Tuple<string, string>>(10);

            //模板项目对应表
            //Dictionary<string, string> Template2Project = new Dictionary<string, string>(10);

            foreach (var str in DBSettings.SelectTables)
            {
                list.Add(new Tuple<string, string>(str.Key, DBSettings.SelectConnectionString));
            }

            //trvDBInfo.selected
            //事件emit
            await Generate(list, GetTemplate2ProjectDict(),progInt);
            loadingBox.Close();
            sw.Stop();
            toolStripStatusLabel1.Text = "耗时:" + sw.Elapsed.TotalSeconds.ToString("f4") + "秒";
            toolStripStatusLabel2.Text = "生成完毕!";
            sw.Stop();
            //MessageBox.Show("生成完毕!");
        }

        private void trvDBInfo_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                if (!DBSettings.SelectTables.ContainsKey(e.Node.Text))
                {
                    DBSettings.SelectTables.Add(e.Node.Text, "1");
                }
            }
            else
            {
                if (DBSettings.SelectTables.ContainsKey(e.Node.Text))
                {
                    DBSettings.SelectTables.Remove(e.Node.Text);
                }
            }

            List<string> tbString = new List<string>();

            foreach (var kv in DBSettings.SelectTables)
            {
                tbString.Add(kv.Key);
            }
            string strTotal = string.Join(",", tbString);
            tbxDTOName.Text = strTotal;

        }

        private void trvDBInfo_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.ImageIndex != 3)
                HideCheckBox(this.trvDBInfo, e.Node);
            e.DrawDefault = true;

        }

        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;
        private void HideCheckBox(TreeView tvw, TreeNode node)
        {

            TVITEM tvi = new TVITEM();

            tvi.hItem = node.Handle;

            tvi.mask = TVIF_STATE;

            tvi.stateMask = TVIS_STATEIMAGEMASK;

            tvi.state = 0;

            SendMessage(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);

        }

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]

        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage; public int cChildren; public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
