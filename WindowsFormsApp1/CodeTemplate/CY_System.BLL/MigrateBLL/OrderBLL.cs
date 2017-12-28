using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

using CY_System.Domain;
using CY_System.WcfService.Interfaces;
using WXFramework.Commons;
using System;
using RestSharp;
using System.Configuration;
using CY_System.Service.Dto;

namespace CY_System.BLL.MigrateBLL
{
    /// <summary>
    /// ##TableDescription##业务逻辑类,该段代码由代码生成器自动生成
    /// 作者: ##Author##
    /// 时间: ##DateTime##
    /// </summary>
    public class ##ModelName##BLL
    {
        private RestClient _##ModelName##ServiceClient;
        private RestClient ##ModelName##ServiceClient
        {
            get
            {
                if (_##ModelName##ServiceClient == null)
                {
                    //url 示例 :http://localhost:8076/api/##ModelName##
                    _##ModelName##ServiceClient = new RestClient(ConfigurationManager.AppSettings["WebApiURL"] + "##ModelName##/");
                    _##ModelName##ServiceClient.Timeout = int.Parse(ConfigurationManager.AppSettings["WebApiTimeOut"]);
                }
                return _##ModelName##ServiceClient;
            }
        }

        /// <summary>
        /// 声明全局接口或类
        /// </summary>
        private static ##ModelName##BLL _instance = new ##ModelName##BLL();

        /// <summary>
        /// 外部接口获取实例
        /// </summary>
        /// <returns></returns>
        public static ##ModelName##BLL GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">要往数据库中的添加的实体对象</param>
        /// <returns>结果</returns>
        public int Add(##ModelName##Dto model)
        {
            RestRequest req = new RestRequest("Add", Method.POST);
            req.AddObject(model);
            var res = ##ModelName##ServiceClient.Execute(req);
            if (res.ErrorMessage != null) throw new Exception(res.ErrorMessage);
            return int.Parse(res.Content);
        }

        /// <summary>
        /// 更新一条##ModelName##记录
        /// </summary>
        /// <param name="model">需要更新的数据</param>
        /// <returns>更新结果</returns>
        public int Update(##ModelName##Dto model)
        {
            RestRequest req = new RestRequest("Update", Method.POST); 
            req.AddObject(model);
            var res = ##ModelName##ServiceClient.Execute(req);
            return int.Parse(res.Content);
        }

        /// <summary>
        /// 获取一条##ModelName##记录
        /// </summary>
        /// <param name="m_id"></param>
        /// <returns></returns>
        public ##ModelName##Dto SelectModel(Guid m_id)
        {
            RestRequest req = new RestRequest("SelectModel/" + m_id.ToString(), Method.GET);
            var res = ##ModelName##ServiceClient.Execute<##ModelName##Dto>(req);
            if (res.ErrorMessage != null) throw new Exception(res.ErrorMessage);
            return res.Data;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="m_id">关键字</param>
        /// <param name="strWhere">条件删除</param>
        /// <returns>删除结果</returns>
        public int Delete(Guid m_id)
        {
            RestRequest req = new RestRequest("Delete/" + m_id.ToString(), Method.DELETE);
            var res = ##ModelName##ServiceClient.Execute(req);
            if (res.ErrorMessage != null) throw new Exception(res.ErrorMessage);
            return int.Parse(res.Content);
        }

        /// <summary>
        /// 根据条件查询##ModelName##记录
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns>实体记录</returns>
        public List<##ModelName##Dto> SelectList()
        {
            RestRequest req = new RestRequest("SelectList", Method.GET);
            var res = ##ModelName##ServiceClient.Execute<List<##ModelName##Dto>>(req);
            if (res.ErrorMessage != null) throw new Exception(res.ErrorMessage);
            return res.Data;
        }

        /// <summary>
        /// 分页查询##ModelName##
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="pageSize">页面显示的行数</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageCount">总行数</param>
        /// <param name="strSort">排序字段</param>
        /// <param name="bAsc">升序或降序</param>
        /// <returns>根据分页查询的数据</returns>
        public List<##ModelName##Dto> SelectByPaged(int pageSize, int pageIndex, out int pageCount, string strSort, bool bAsc)
        {
            RestRequest req = new RestRequest("SelectByPaged", Method.GET);
            req.AddParameter("pageSize", pageSize);
            req.AddParameter("pageIndex", pageIndex);
            req.AddParameter("strSort", strSort);
            req.AddParameter("bAsc", bAsc);
            var res = ##ModelName##ServiceClient.Execute<PagedDto<##ModelName##Dto>>(req);
            if (res.ErrorMessage != null) throw new Exception(res.ErrorMessage);
            var returnedData = res.Data;
            pageCount = returnedData.PageCount;
            return returnedData.Data;
        }
    }
}
