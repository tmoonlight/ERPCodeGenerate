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
    /// ##TableDescription##操作控制器
    /// </summary>
    [Produces("application/json")]
    [Route("api/##ModelName##")]
    public class ##ModelName##Controller : Controller
    {
        //##TableDescription##仓库
        private ##ModelName##Repository repository = new ##ModelName##Repository();
        /// <summary>
        /// 判断##TableDescription##是否存在
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
        /// 新增##TableDescription##
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public int Add(##ModelName##Dto model)
        {
            ##ModelName##Info dto = model.MapTo<##ModelName##Info>();
            return repository.Add(dto);
        }

        /// <summary>
        /// 修改##TableDescription##字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public int Update(##ModelName##Dto model)
        {
            return repository.Update(model.MapTo<##ModelName##Info>());
        }

        /// <summary>
        /// 删除##TableDescription##,物理删除.
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
        /// 查询单个##TableDescription##实体
        /// </summary>
        /// <param name="m_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectModel/{m_id}")]
        public ##ModelName##Dto SelectModel(Guid m_id)
        {
            return repository.SelectModel(m_id).MapTo<##ModelName##Dto>();
        }

        /// <summary>
        /// 查询##TableDescription##列表
        /// </summary>
        /// <param name="predicateExpression"></param>
        /// <response code="205">查不到任何数据</response>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectList")]
        [ProducesResponseType(205)]
        [ProducesResponseType(typeof(IEnumerable<##ModelName##Dto>), 200)]
        public IEnumerable<##ModelName##Dto> SelectList(string predicateExpression)
        {
            int nouseint = 0;
            //最大10条
            IEnumerable<##ModelName##Info> result = repository.SelectByPaged(null, 10, 1, out nouseint, "ID", true);
            return result.MapToList<##ModelName##Info, ##ModelName##Dto>();
        }

        /// <summary>
        /// ##TableDescription##
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strSort"></param>
        /// <param name="bAsc"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectByPaged")]
        [ProducesResponseType(205)]
        [ProducesResponseType(typeof(IEnumerable<##ModelName##Dto>), 200)]
        public PagedDto<##ModelName##Dto> SelectByPaged(int pageSize, int pageIndex, string strSort, bool bAsc)
        {
            int pageCount = 0;
            PagedDto<##ModelName##Dto> dto = new PagedDto<##ModelName##Dto>();
            dto.Data = repository.SelectByPaged(null, pageSize, pageIndex, out pageCount, strSort, bAsc).MapToList<##ModelName##Info, ##ModelName##Dto>().ToList();
            dto.PageCount = pageCount;
            return dto;
        }

    }
}