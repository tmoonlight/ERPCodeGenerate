using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CY_System.CodeBuilder
{
    public partial class SelectForms : Form
    {
        Dictionary<string, string> selectTables = new Dictionary<string, string>();
        List<string> dataBase = new List<string>();
        string selectDataBase = "";
        string selectConnectionString = "";

        public SelectForms()
        {
            InitializeComponent();

            foreach (DBConfig _DBConfig in DBSettings.DataBaseConfigList)
            {
                dataBase.Add(_DBConfig.DataBase);
            }

            cboDataBase.DataSource = dataBase;
            cboDataBase.SelectedIndex = 0;

            foreach (DBConfig _DBConfig in DBSettings.DataBaseConfigList)
            {
                if (_DBConfig.DataBase == cboDataBase.Text)
                {
                    selectDataBase = _DBConfig.DataBase;
                    selectConnectionString = _DBConfig.ConString;
                }
            }
        }

        #region 窗体初始化加载数据
        /// <summary>
        /// 窗体初始化加载数据
        /// </summary>
        private void LoadData(string strXType)
        {
            lvSelectList.Items.Clear();
            lvNotSelectList.Items.Clear();

            DataTable _tables = SQLServerDBHelper.GetTables(selectConnectionString, strXType);

            foreach (DataRow _tableNameRow in _tables.Rows)
            {
                string _tableName = _tableNameRow["name"].ToString();
                ListViewItem lvi = new ListViewItem(_tableName);
                lvi.Tag = _tableNameRow["xtype"];
                lvNotSelectList.Items.Add(lvi);
            }
        }
        #endregion

        #region 全选按钮事件
        /// <summary>
        /// 全选按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvNotSelectList.Items)
            {
                ListViewItem lviNew = lvi.Clone() as ListViewItem;
                selectTables.Add(lvi.Text, lvi.Tag.ToString());
                lvSelectList.Items.Add(lviNew);
                lvNotSelectList.Items.Remove(lvi);
            }
        }
        #endregion

        #region 单选按钮事件
        /// <summary>
        /// 单选按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAngle_Click(object sender, EventArgs e)
        {
            if (lvNotSelectList.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvi in lvNotSelectList.SelectedItems)
                {
                    ListViewItem lviNew = lvi.Clone() as ListViewItem;
                    selectTables.Add(lvi.Text, lvi.Tag.ToString());
                    lvSelectList.Items.Add(lviNew);
                    lvNotSelectList.Items.Remove(lvi);
                }
            }
        }
        #endregion

        #region 全部撤销选择事件
        /// <summary>
        /// 全部撤销选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllCancel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvSelectList.Items)
            {
                ListViewItem lviNew = lvi.Clone() as ListViewItem;
                selectTables.Remove(lvi.Text);
                lvNotSelectList.Items.Add(lviNew);
                lvSelectList.Items.Remove(lvi);
            }
        }
        #endregion

        #region 单项撤销事件
        /// <summary>
        /// 单项撤销事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAngleCancel_Click(object sender, EventArgs e)
        {
            if (lvSelectList.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvi in lvSelectList.SelectedItems)
                {
                    ListViewItem lviNew = lvi.Clone() as ListViewItem;
                    selectTables.Remove(lvi.Text);
                    lvNotSelectList.Items.Add(lviNew);
                    lvSelectList.Items.Remove(lvi);
                }
            }
        }
        #endregion

        #region 数据库选择改变发生
        /// <summary>
        /// 数据库选择改变发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboDataBase_TextChanged(object sender, EventArgs e)
        {
            lvSelectList.Items.Clear();
            lvNotSelectList.Items.Clear();

            foreach (DBConfig _DBConfig in DBSettings.DataBaseConfigList)
            {
                if (_DBConfig.DataBase == cboDataBase.Text)
                {
                    selectDataBase = _DBConfig.DataBase;
                    selectConnectionString = _DBConfig.ConString;
                }
            }

            DataTable _tables = SQLServerDBHelper.GetTables(selectConnectionString, "U");

            foreach (DataRow _tableNameRow in _tables.Rows)
            {
                string _tableName = _tableNameRow["name"].ToString();
                ListViewItem lvi = new ListViewItem(_tableName);
                lvNotSelectList.Items.Add(lvi);
            }
        }
        #endregion

        #region 确定按钮事件
        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            ///生成代码
            if (selectTables.Count > 0)
            {
                DBSettings.SelectTables = selectTables;
                DBSettings.SelectDataBase = selectDataBase;
                DBSettings.SelectConnectionString = selectConnectionString;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.None;
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
            this.Close();
        }
        #endregion

        /// <summary>
        /// 选择项改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkListType_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string strXType = string.Empty;
            for (int i = 0; i < chkListType.Items.Count; i++)
            {
                if ((e.Index != i && chkListType.GetItemChecked(i))
                    || (e.Index == i && e.NewValue == CheckState.Checked))
                {
                    switch (chkListType.Items[i].ToString())
                    {
                        case "数据表":
                            strXType += "U,";
                            break;
                        case "视图":
                            strXType += "V,";
                            break;
                        case "存储过程":
                            strXType += "P,";
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(strXType))
            {
                strXType = strXType.Substring(0, strXType.Length - 1);
                strXType = strXType.Replace(",", "','");
            }

            this.LoadData(strXType);
        }
    }
}
