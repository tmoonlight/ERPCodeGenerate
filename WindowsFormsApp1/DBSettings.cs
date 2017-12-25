using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CY_System.CodeBuilder
{
    /// <summary>
    /// 数据库信息设置类
    /// </summary>
    public class DBSettings
    {

        #region 生成代码的信息储存
        static Dictionary<string, string> selectTables = new Dictionary<string, string>();
        /// <summary>
        /// 选中的表格
        /// </summary>
        public static Dictionary<string, string> SelectTables
        {
            get { return selectTables; }
            set { selectTables = value; }
        }

        static string selectDataBase = "";
        /// <summary>
        /// 选中的数据库
        /// </summary>
        public static string SelectDataBase
        {
            get { return selectDataBase; }
            set { selectDataBase = value; }
        }
        static string selectConnectionString = "";
        /// <summary>
        /// 选中的链接字符串
        /// </summary>
        public static string SelectConnectionString
        {
            get { return selectConnectionString; }
            set { selectConnectionString = value; }
        }
        #endregion

        private static List<DBConfig> dataBaseConfigList = new List<DBConfig>();
        /// <summary>
        /// 数据库列表 
        /// </summary>
        public static List<DBConfig> DataBaseConfigList
        {
            get { return dataBaseConfigList; }
        }
        /// <summary>
        /// 将数据库信息添加到列表中
        /// </summary>
        /// <param name="_DBConfig"></param>
        public static void AddConfigLiat(DBConfig _DBConfig)
        {
            dataBaseConfigList.Add(_DBConfig);
        }

        /// <summary>
        /// 判断列表中是否存在该数据库记录
        /// </summary>
        /// <returns></returns>
        public static bool Exist(DBConfig _DBConfig)
        {
            foreach (DBConfig _DBConfigItem in dataBaseConfigList)
            {
                if (_DBConfigItem.DataBase == _DBConfig.DataBase && _DBConfigItem.ServerName == _DBConfig.ServerName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="_ServerName">服务器名称</param>
        /// <param name="_DataBase">数据库名称</param>
        /// <returns></returns>
        public static string GetConString(string _ServerName, string _DataBase)
        {
            foreach (DBConfig _DBConfigItem in dataBaseConfigList)
            {
                if (_DBConfigItem.DataBase == _DataBase && _DBConfigItem.ServerName == _ServerName)
                {
                    return _DBConfigItem.ConString;
                }
            }
            return "";
        }
    }
}
