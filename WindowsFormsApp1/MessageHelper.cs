using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CY_System.CodeBuilder
{
    /// <summary>
    /// 消息帮助类
    /// </summary>
    public class MessageHelper
    {
        /// <summary>
        /// 错误消息显示
        /// </summary>
        /// <param name="_msg"></param>
        public static void ErrorMessageShow(string _msg)
        {
            MessageBox.Show(_msg ,"遇到错误：",MessageBoxButtons.OK ,MessageBoxIcon.Error );
        }
        /// <summary>
        /// 警告消息显示
        /// </summary>
        /// <param name="_msg"></param>
        public static void WarningMessageShow(string _msg) 
        {
            MessageBox.Show(_msg ,"警告：",MessageBoxButtons.OK ,MessageBoxIcon.Warning );
        }
        /// <summary>
        /// 成功消息显示
        /// </summary>
        /// <param name="_msg"></param>
        public static void SucceedMessageShow(string _msg) 
        {
            MessageBox.Show(_msg ,"操作成功：",MessageBoxButtons.OK ,MessageBoxIcon.None );
        }
        /// <summary>
        /// 未知错误消息显示
        /// </summary>
        /// <param name="_msg"></param>
        public static void UnKnowErrorMessageShow(string _msg) 
        {
            MessageBox.Show(_msg ,"遇到未知错误",MessageBoxButtons.OK ,MessageBoxIcon .Stop);
        }
    }
}
