using CY_System.DomainStandard;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;

namespace CY_System.Infrastructure.Repository
{
    /// <summary>
    /// 订单仓库
    /// </summary>
    public class OrderRepository : BaseRepository_CA<OrderInfo>
    {

        public IEnumerable<T> TQuery<T>(IDbConnection connection, Func<T> typeBuilder, string sql)
        {
            return connection.Query<T>(sql);
        }
        //ToDo:具体的对数据库实现的方法写在这里
       
    }
}
