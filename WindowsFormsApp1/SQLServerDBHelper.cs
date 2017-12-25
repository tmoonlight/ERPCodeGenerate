using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CY_System.CodeBuilder
{
    /// <summary>
    /// SQLServer数据帮助类
    /// </summary>
    public class SQLServerDBHelper
    {
        #region 查询数据库中的所有数据库
        /// <summary>
        /// 查询数据库中的所有数据库
        /// </summary>
        /// <param name="_conString">连接字符串</param>
        /// <returns></returns>
        public static List<string> GetDataBase(string _conString)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(_conString);
            SqlDataAdapter adpt = new SqlDataAdapter("select [name] from sysdatabases", conn);
            List<string> _DataBaseList = new List<string>();
            try
            {
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow _DataRowItem in dt.Rows)
                    {
                        _DataBaseList.Add(_DataRowItem["name"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                MessageHelper.ErrorMessageShow("对不起：\n\t连接数据库失败,请确认您的用户名和密码，稍后请重试...");
            }
            return _DataBaseList;
        }
        #endregion

        #region 查询数据库中的所有表、存储过程、视图
        /// <summary>
        /// 查询数据库中的所有表、存储过程、视图
        ///<param name="_conString" >连接字符串</param>
        ///<param name="_type" >类型，U为表，V为视图，P为存储过程</param>
        /// <returns></returns>
        public static DataTable GetTables(string _conString, string _type)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(_conString);
            string cmdString = "select [name],xtype from sysobjects where [name]<>'sysdiagrams' and [name] not like 'sp_%' and xtype in('" + _type + "') order by xtype,[name]";
            SqlDataAdapter adpt = new SqlDataAdapter(cmdString, conn);
            try
            {
                adpt.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageHelper.ErrorMessageShow(ex.Message.ToString());
            }
            return dt;
        }
        #endregion

        #region 返回表结构详细信息
        /// <summary>
        /// 返回表结构详细信息
        /// </summary>
        /// <param name="_conString">连接字符串</param>
        /// <param name="_dtName">表名</param>
        /// <returns></returns>
        public static DataTable GetTableInfo(string _conString, string _dtName)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(_conString);
            string cmdString = @"SELECT 
                                colorder=C.column_id,
                                ColumnName=C.name,
                                TypeName=T.name, 
                                Length=CASE WHEN T.name='nchar' THEN C.max_length/2 WHEN T.name='nvarchar' THEN C.max_length/2 ELSE C.max_length END,
                                Preci=C.precision, 
                                Scale=C.scale, 
                                IsIdentity=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END,
                                isPK=ISNULL(IDX.PrimaryKey,N''),
                                Computed=CASE WHEN C.is_computed=1 THEN N'√'ELSE N'' END, 
                                IndexName=ISNULL(IDX.IndexName,N''), 
                                IndexSort=ISNULL(IDX.Sort,N''), 
                                Create_Date=O.Create_Date, 
                                Modify_Date=O.Modify_date,
                                cisNull=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END, 
                                defaultVal=ISNULL(D.definition,N''), 
                                deText=ISNULL(PFD.[value],N'') 
                                			
                                FROM sys.columns C 
                                INNER JOIN sys.objects O 
                                ON C.[object_id]=O.[object_id] 
                                AND (O.type='U' or O.type='V') 
                                AND O.is_ms_shipped=0 
                                INNER JOIN sys.types T 
                                ON C.user_type_id=T.user_type_id 
                                LEFT JOIN sys.default_constraints D 
                                ON C.[object_id]=D.parent_object_id 
                                AND C.column_id=D.parent_column_id 
                                AND C.default_object_id=D.[object_id] 
                                LEFT JOIN sys.extended_properties PFD 
                                ON PFD.class=1  
                                AND C.[object_id]=PFD.major_id  
                                AND C.column_id=PFD.minor_id
                                LEFT JOIN -- 索引及主键信息
                                ( 
                                SELECT  
                                IDXC.[object_id], 
                                IDXC.column_id, 
                                Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending') 
                                WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END, 
                                PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END, 
                                IndexName=IDX.Name 
                                FROM sys.indexes IDX 
                                INNER JOIN sys.index_columns IDXC 
                                ON IDX.[object_id]=IDXC.[object_id] 
                                AND IDX.index_id=IDXC.index_id 
                                LEFT JOIN sys.key_constraints KC 
                                ON IDX.[object_id]=KC.[parent_object_id] 
                                AND IDX.index_id=KC.unique_index_id 
                                INNER JOIN  -- 对于一个列包含多个索引的情况,只显示第1个索引信息
                                ( 
                                SELECT [object_id], Column_id, index_id=MIN(index_id) 
                                FROM sys.index_columns 
                                GROUP BY [object_id], Column_id 
                                ) IDXCUQ 
                                ON IDXC.[object_id]=IDXCUQ.[object_id] 
                                AND IDXC.Column_id=IDXCUQ.Column_id 
                                AND IDXC.index_id=IDXCUQ.index_id 
                                ) IDX 
                                ON C.[object_id]=IDX.[object_id] 
                                AND C.column_id=IDX.column_id

                                WHERE O.name=N'dtName' 
                                ORDER BY O.name,C.column_id  ";

            cmdString = cmdString.Replace("dtName", _dtName);
            SqlDataAdapter adpt = new SqlDataAdapter(cmdString, conn);

            try
            {
                adpt.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageHelper.ErrorMessageShow(ex.Message.ToString());
            }

            if (dt.Rows.Count <= 0) MessageHelper.WarningMessageShow("检查到表格" + _dtName + "没有任何结构的表格，系统将自动忽略它");

            return dt;

        }
        #endregion

        #region 获取数据库的详细信息
        /// <summary>
        /// 获取数据库的详细信息
        /// </summary>
        /// <param name="conString">链接字符串</param>
        /// <returns>数据库的详细信息</returns>
        public static DataTable GetDataBaseInfo(string conString)
        {
            string sqlString = @"select name ,'类型'=case 
								                            when xtype='U' then '表'
								                            when xtype ='V' then '视图'
								                            when xtype ='P' then '存储过程'
								                            end
                                 ,crdate  from sys.sysobjects where xtype in('U','V','P')
                                 
                                order by '类型',name";

            SqlConnection conn = new SqlConnection(conString);
            SqlDataAdapter adpt = new SqlDataAdapter(sqlString, conn);
            DataTable dt = new DataTable();

            try
            {
                adpt.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageHelper.UnKnowErrorMessageShow(ex.ToString());
            }
            return dt;
        }
        #endregion

        #region 查询表中数据
        /// <summary>
        /// 查询表中数据
        /// </summary>
        /// <param name="_connectionString">链接字符串</param>
        /// <param name="_tableName">表明</param>
        /// <returns></returns>
        public static DataTable GetData(string _connectionString, string _tableName)
        {
            string sqlString = "select * from [" + _tableName + "]";

            SqlConnection conn = new SqlConnection(_connectionString);

            SqlDataAdapter adpt = new SqlDataAdapter(sqlString, conn);

            DataTable dt = new DataTable();

            try
            {
                adpt.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageHelper.UnKnowErrorMessageShow(ex.ToString());
            }

            return dt;
        }
        #endregion

        #region 获取insert的数据脚本
        /// <summary>
        /// 获取insert的数据脚本
        /// </summary>
        /// <param name="_connectionString">连接字符串</param>
        /// <param name="_tableName" >表名</param>
        /// <returns></returns>
        public static DataTable GetInsetScript(string _connectionString, string _tableName)
        {
            string _sqlString = @"declare @column varchar(2000)    
                                    declare @columndata varchar(2000)    
                                    declare @sql varchar(8000)    
                                    declare @xtype tinyint    
                                    declare @name sysname    
                                    declare @objectId int    
                                    declare @objectname sysname    
                                    declare @ident int    
                                    set nocount on    
                                    -- 判断对象是否存在    
                                    set @objectId=object_id('@tabelname@')    

                                    --此判断不严密    
                                    set @objectname=rtrim(object_name(@objectId))    

                                    declare syscolumns_cursor cursor   
                                    for select c.name,c.xtype from syscolumns c where c.id=@objectid order by c.colid    
                                    open syscolumns_cursor    
                                    set @column=''    
                                    set @columndata=''    
                                    fetch next from syscolumns_cursor into @name,@xtype    
                                    while @@fetch_status < >-1    
                                      begin    
                                        if @@fetch_status < >-2    
                                          begin    
                                            --if @xtype not in(189,34,35,99,98) --timestamp不需处理，image,text,ntext,sql_variant 暂时不处理    
                                              begin    
                                                set @column=@column+case when len(@column)=0 then'' else ','end+@name     
                                                set @columndata=@columndata+case when len(@columndata)=0 then '' else ','','','end    
                                                    +case when @xtype in(167,175) then '''''''''+'+@name+'+''''''''' --varchar,char    
                                                          when @xtype in(231,239) then '''N''''''+'+@name+'+''''''''' --nvarchar,nchar    
                                                          when @xtype=61 then '''''''''+convert(char(23),'+@name+',121)+''''''''' --datetime    
                                                          when @xtype=58 then '''''''''+convert(char(16),'+@name+',120)+''''''''' --smalldatetime    
                                                          when @xtype=36 then '''''''''+convert(char(36),'+@name+')+''''''''' --uniqueidentifier    
                                                          else @name end    
                                              end    
                                          end    
                                        fetch next from syscolumns_cursor into @name,@xtype    
                                      end    
                                    close syscolumns_cursor    
                                    deallocate syscolumns_cursor    
                                    set @sql='set nocount on select ''insert '+'@tabelname@'+'('+@column+') values(''as ''--'','+@columndata+','')'' from '+'@tabelname@'    
                                    exec(@sql)  
                                    ";

            _sqlString = _sqlString.Replace("@tabelname@", _tableName);
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter adpt = new SqlDataAdapter(_sqlString, _connectionString);
            DataTable dt = new DataTable();

            try
            {
                adpt.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageHelper.WarningMessageShow(ex.ToString());
            }

            return dt;

        }
        #endregion

        #region 删除表/视图/存储过程
        /// <summary>
        /// 删除表/视图/存储过程
        /// </summary>
        /// <param name="_tableName">表名</param>
        /// <param name="type">类型：U为表，V为视图，P为存储过程</param>
        /// <param name="_connectionString">连接字符串</param>
        /// <returns></returns>
        public static bool DeleteTable(string _tableName, string type, string _connectionString)
        {
            string _sql = "";

            switch (type)
            {
                case "U": _sql = "DROP TABLE [" + _tableName + "]"; break;
                case "V": _sql = "DROP VIEW [" + _tableName + "]"; break;
                case "P": _sql = "DROP PROC [" + _tableName + "]"; break;
                default:
                    _sql = "DROP TABLE [" + _tableName + "]"; break;
            }

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(_sql, conn);

            try
            {
                conn.Open();
                int res = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }

            if (isDelete(_tableName, type, _connectionString)) return true;
            else return false;
        }

        public static bool isDelete(string _name, string _type, string _conString)
        {
            string _sql = "";

            switch (_type)
            {
                case "U": _sql = "select COUNT(*) from sysobjects where  xtype='U' and name ='@tableName@'"; break;
                case "V": _sql = "select COUNT(*) from sysobjects where  xtype='V' and name ='@tableName@'"; break;
                case "P": _sql = "select COUNT(*) from sysobjects where  xtype='P' and name ='@tableName@'"; break;
                default:
                    _sql = "select * from sysobjects where  xtype='U' and name ='@tableName@' "; break;
            }

            SqlConnection conn = new SqlConnection(_conString);
            SqlCommand cmd = new SqlCommand(_sql, conn);
            int res = -1;
            try
            {
                conn.Open();
                res = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }
            if (res <= 0) return true;
            else return false;
        }
        #endregion

        #region 获取视图定义
        /// <summary>
        /// 获取视图定义
        /// </summary>
        /// <param name="_conStr">连接字符串</param>
        /// <param name="_viewName">视图名</param>
        /// <returns></returns>
        public static DataTable GetViewDefine(string _conStr, string _viewName)
        {
            string sql = "select text from syscomments vi join sysobjects obj  on vi.id=obj .id where name='@ViewName@' and obj .type ='V'";
            sql = sql.Replace("@ViewName@", _viewName);

            SqlConnection conn = new SqlConnection(_conStr);
            SqlDataAdapter adpt = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();

            try
            {
                adpt.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageHelper.WarningMessageShow("未知错误消息：\n" + ex.ToString());
            }

            return dt;
        }
        #endregion

        #region 查看存储过程定义
        /// <summary>
        /// 查看存储过程定义
        /// </summary>
        /// <param name="_ProcName">存储过程名字</param>
        /// <param name="_ConStr">连接字符串</param>
        /// <returns></returns>
        public static DataTable GetProcDefine(string _ProcName, string _ConStr)
        {
            string sql = "select text   from syscomments vi join sysobjects obj on vi.id=obj.id where name='@procName@' and obj .type ='P'";
            sql = sql.Replace("@procName@", _ProcName);

            SqlConnection conn = new SqlConnection(_ConStr);
            SqlDataAdapter adpt = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();

            try
            {
                adpt.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageHelper.WarningMessageShow("遇到未知错误：\n" + ex.ToString());
            }

            return dt;
        }
        #endregion
    }
}