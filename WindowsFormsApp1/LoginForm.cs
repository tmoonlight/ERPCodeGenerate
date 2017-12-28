using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CY_System.CodeBuilder
{
    public partial class LoginForm : Form
    {
        private DBConfig _DBConfig;
        private StringBuilder _conString;
        private bool isPostBack = false;
        //MainForm _MainForm;

        //public MainForm MainForm
        //{
        //    get { return _MainForm; }
        //    set { _MainForm = value; }
        //}

        public bool IsPostBack
        {
            get { return isPostBack; }
            set { isPostBack = value; }
        }
        List<string> _DataBaseList;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginForm() : this(false)
        {

        }

        public LoginForm(bool pIsPostBack)
        {
            this.isPostBack = pIsPostBack;
            InitializeComponent();
            DataLoad();
        }
        /// <summary>
        /// 窗体初始化数据加载
        /// </summary>
        private void DataLoad()
        {
            txtLoginName.Enabled = false;
            txtPwd.Enabled = false;
            cboServerName.SelectedIndex = 0;
            cboValidataType.SelectedIndex = 0;
        }
        #endregion

        #region 测试按钮事件
        /// <summary>
        /// 测试按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheck_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 验证类型改变发生
        /// <summary>
        /// 验证类型改变发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboValidataType_TextChanged(object sender, EventArgs e)
        {
            if (cboValidataType.Text.Trim() == "Windows   身份认证")
            {
                txtLoginName.Enabled = false;
                txtPwd.Enabled = false;
            }
            else
            {
                txtLoginName.Enabled = true;
                txtPwd.Enabled = true;
            }
        }
        #endregion

        #region 确定按钮事件
        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            _DBConfig = new DBConfig();
            _conString = new StringBuilder();
            _DataBaseList = new List<string>();

            if (cboServerName.Text == "Local(本机)")
            {
                _DBConfig.ServerName = "Local(本机)";
                _conString.Append("Data Source=.;");
            }
            else
            {
                _DBConfig.ServerName = "Local\\SQLExpress(本机\\SQLExpress)";
                _conString.Append("server=").Append(cboServerName.Text).Append(";");  //这里做了修改，可以自己添加登录实例名称
                //_conString.Append("Data Source=.\\SQLExpress;");
            }

            //身份认证类型
            if (cboValidataType.Text == "Windows   身份认证")
            {
                _DBConfig.ValidataType = "Windows   身份认证";
                _conString.Append("Initial Catalog=master;");
                _conString.Append("Integrated Security=SSPI;");
            }
            else
            {
                _DBConfig.ValidataType = "SQL Server身份认证";
                //_conString.Append("Initial Catalog=master;");
                _conString.Append("database=master;");
                _conString.Append("User ID=" + txtLoginName.Text + ";Password=" + txtPwd.Text + ";");
                string str = _conString.ToString();
            }

            _DataBaseList = SQLServerDBHelper.GetDataBase(_conString.ToString());

            if (_DataBaseList.Count > 0)
            {
                //_DataBaseList.Add("全部数据库");
                //cboDataBase.DataSource = _DataBaseList;
                //cboDataBase.Enabled = true;
                //cboDataBase.SelectedIndex = 0;
            }
            else
            {
                //cboDataBase.Enabled = false;
                //cboDataBase.Text = "";
                //cboDataBase.DataSource = null;
            }
            //直接把所有库查出来
            //if (cboDataBase.Text.Trim() != "")
            //{
            ////全部或多个库
            //if (cboDataBase.Text.Trim() == "全部数据库")
            //{
            foreach (string _strItem in _DataBaseList)
            {
                DBConfig _NewDBConfig = new DBConfig();

                //if (_strItem == "全部数据库") continue;
                string conString = _conString.ToString();
                //直接replace master,需要改进
                conString = conString.Replace("master", _strItem);
                _NewDBConfig.ConString = conString;
                _NewDBConfig.DataBase = _strItem;
                _NewDBConfig.ServerName = _DBConfig.ServerName;
                _NewDBConfig.ServerType = _DBConfig.ServerType;
                _NewDBConfig.UserName = _DBConfig.UserName;
                _NewDBConfig.UserPwd = _DBConfig.UserPwd;
                _NewDBConfig.ValidataType = _DBConfig.ValidataType;

                if (!DBSettings.Exist(_NewDBConfig))
                    DBSettings.AddConfigLiat(_NewDBConfig);
            }
            //}
            //else
            //{
            //    DBConfig _NewDBConfig = new DBConfig();
            //    //直接replace master,需要改进
            //    _NewDBConfig.ConString = _conString.ToString().Replace("master", cboDataBase.Text.Trim());
            //    _NewDBConfig.DataBase = cboDataBase.Text.Trim();
            //    _NewDBConfig.ServerName = _DBConfig.ServerName;
            //    _NewDBConfig.ServerType = _DBConfig.ServerType;
            //    _NewDBConfig.UserName = _DBConfig.UserName;
            //    _NewDBConfig.UserPwd = _DBConfig.UserPwd;
            //    _NewDBConfig.ValidataType = _DBConfig.ValidataType;

            //    if (!DBSettings.Exist(_NewDBConfig))
            //        DBSettings.AddConfigLiat(_NewDBConfig);
            //}
            ////如果回传
            //if (isPostBack)
            //{
            //    this.Hide();
            //    _MainForm.LoadData();
            //}
            //else
            //{
            //    this.Hide();
            //    _MainForm = new MainForm(this);
            //    _MainForm.Show();
            //}
            //    SelectForms sf = new SelectForms();
            //    sf.Show();
            //}
            //else
            //{

            //}
            //else
            //{
            //    MessageHelper.WarningMessageShow("请先测试连接，稍后重试！");
            //}

            this.DialogResult = DialogResult.Cancel;
            if (IsPostBack)
            {
                this.Close();
            }
            else
            {
                var gen = new frmGenerator();
                //传递事件到外层
                gen.Generate += (list, dict, p) => Generate(list, dict, p);
                gen.Show();
                this.Hide();
            }

        }



        #endregion

        #region 关闭按钮事件
        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            if (isPostBack)
            {
                this.Hide();
            }
            else this.Close();
        }
        #endregion

        #region 窗口关闭事件
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (isPostBack)
            //{
            //    e.Cancel = true;
            //    this.Hide();
            //}
            //else Application.Exit();
        }
        #endregion



        public event GenerateHandler Generate;

        public delegate Task<bool> GenerateHandler(List<Tuple<string, string,string>> TableNames, Dictionary<string, string> dict, IProgress<int> progress);

    }
}
