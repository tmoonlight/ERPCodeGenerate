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
        public IEnumerable<dynamic> SelectOrderDetail()
        {

            IEnumerable<OrderInfoWithOrderCount> returnvalue = null;
            try
            {
                using (var conn = GetConnection())
                {
                    returnvalue = conn.Query<OrderInfo, OrderInfoWithOrderCount, OrderInfoWithOrderCount>(@"SELECT   ca_Order.ID 
                                        COUNT(*) ItemCount
                               FROM     ca_Order,
                                        ca_OrderMatters
                               WHERE    ca_Order.ID = ca_OrderMatters.PID
                               GROUP BY dbo.ca_Order.ID", (order1, order2) => { order2.Orderinfo = order1; return order2; });

                    
                    var returnvalue2 = TQuery(conn,() => new
                    {
                        ContactId = default(int),
                        Name = default(string),
                    }, @"SELECT   ca_Order.ID 
                                        COUNT(*) ItemCount
                               FROM     ca_Order,
                                        ca_OrderMatters
                               WHERE    ca_Order.ID = ca_OrderMatters.PID
                               GROUP BY dbo.ca_Order.ID");

                    return returnvalue;
                }
            }
            catch (Exception ex)
            {

            }
            return returnvalue;
        }

        //ToDo:具体的对数据库实现的方法写在这里
        public dynamic SelectOrderDetail2()
        {
            using (var conn = GetConnection())
            {

                var returnvalue = conn.Query(@"SELECT   ca_Order.ID ,
                                        COUNT(*) ItemCount
                               FROM     ca_Order,
                                        ca_OrderMatters
                               WHERE    ca_Order.ID = ca_OrderMatters.PID
                               GROUP BY dbo.ca_Order.ID");


                return returnvalue;
            }

            return new { x = 1, y = 2 };
        }
    }
}
