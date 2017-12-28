using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CY_System.DomainStandard
{
    /// <summary>
	/// ##TableDescription##实体类
    /// 作者: ##Author##
    /// 时间: ##DateTime##
    /// </summary>
    [POCO(DbConnName = CY_SystemConsts.ConnectionString_conn_ca, TableName = "##TableName##")]
    public class ##ModelName##Info
    {
##Fields##
    }
}

