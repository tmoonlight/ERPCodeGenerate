using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CY_System.DomainStandard;
using CY_System.Infrastructure;
using CY_System.Service.Dto;
using CY_System.Service.Extensions;
using CY_System.Infrastructure.Repository;
using AutoMapper;
using System.Dynamic;

namespace CY_System.Service.Controllers.CA
{
    /// <summary>
    /// {订单}操作控制器
    /// </summary>
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller//,IRepository<OrderInfo>
    {
        //{订单}仓库
        private OrderRepository repository = new OrderRepository();
        /// <summary>
        /// 判断{订单}是否存在
        /// </summary>
        /// <param name="m_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Exists/{m_id}")]
        public bool Exists(Guid m_id)
        {
            return repository.Exists(m_id);
        }

        /// <summary>
        /// 新增{订单}
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public int Add(OrderDto model)
        {
            OrderInfo dto = model.MapTo<OrderInfo>();
            return repository.Add(dto);
        }

        /// <summary>
        /// 修改{订单}字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public int Update(OrderDto model)
        {
            return repository.Update(model.MapTo<OrderInfo>());
        }

        /// <summary>
        /// 删除{订单},物理删除.
        /// </summary>
        /// <param name="m_id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete/{m_id}")]
        public int Delete(Guid m_id)
        {
            return repository.Delete(m_id);
        }

        /// <summary>
        /// 查询单个{订单}实体
        /// </summary>
        /// <param name="m_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectModel/{m_id}")]
        public OrderDto SelectModel(Guid m_id)
        {
            return repository.SelectModel(m_id).MapTo<OrderDto>();
        }

        /// <summary>
        /// 查询{订单}列表
        /// </summary>
        /// <param name="predicateExpression"></param>
        /// <response code="205">查不到任何数据</response>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectList")]
        [ProducesResponseType(205)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
        public IEnumerable<OrderDto> SelectList(string predicateExpression)
        {
            int nouseint = 0;
            //最大10条
            IEnumerable<OrderInfo> result = repository.SelectByPaged(null, 10, 1, out nouseint, "ID", true);
            return result.MapToList<OrderInfo, OrderDto>();
        }

        /// <summary>
        /// {订单}
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strSort"></param>
        /// <param name="bAsc"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectByPaged")]
        [ProducesResponseType(205)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
        public PagedDto<OrderDto> SelectByPaged(int pageSize, int pageIndex, string strSort, bool bAsc)
        {
            int pageCount = 0;
            PagedDto<OrderDto> dto = new PagedDto<OrderDto>();
            dto.Data = repository.SelectByPaged(null, pageSize, pageIndex, out pageCount, strSort, bAsc).MapToList<OrderInfo, OrderDto>().ToList();
            dto.PageCount = pageCount;
            return dto;
        }

    }
}