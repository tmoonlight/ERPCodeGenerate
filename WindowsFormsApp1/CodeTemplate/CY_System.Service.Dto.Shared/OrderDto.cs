using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CY_System.Service.Dto
{
    /// <summary>
    /// 该段代码由代码生成器自动生成
    /// 作者：田大伟
    /// 时间：2016/8/30 10:10:04
    /// </summary>
    
    public class OrderDto
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderDto()
        {
            this.ID = null;
            this.CACode = string.Empty;
            this.Cus_ID = null;
            this.Status = string.Empty;
            this.OrderFrom = string.Empty;
            this.iTotalCost = 0;
            this.CurState = TState.None;
            this.VerificationCode = string.Empty;
            this.iPrintCount = 0;
            this.isElectronic = false;
        }

        /// <summary>
        /// 
        /// <summary>
        
        public Guid? ID { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string CACode { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public Guid? Cus_ID { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string Status { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string OrderFrom { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string HandleType { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string cCusCode { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string cContactCode { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string CusName { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string LinkTel { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string Linkman { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string PayType { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public bool IsCurPay { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public double? CurPayCost { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public double? iTotalCost { get; set; }

        /// <summary>
        /// 客户信息类型(0-注册  1-其他  2-变更)
        /// <summary>
        
        public int? CusDataType { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public bool? IsInvoice { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public Guid? InvoiceID { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string CreatedBy { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string cDepCode { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string cTeamCode { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 开单部门
        /// <summary>
        
        public string cDepName { get; set; }

        /// <summary>
        /// 营业部
        /// <summary>
        
        public string cTeamName { get; set; }

        /// <summary>
        /// 创建人
        /// <summary>
        
        public string cPsn_Name { get; set; }

        /// <summary>
        /// 状态
        /// <summary>
        
        public string StatusName { get; set; }

        /// <summary>
        /// 已付总额
        /// <summary>
        
        public double? PaidCost { get; set; }

        /// <summary>
        /// 验证码
        /// <summary>
        
        public string VerificationCode { get; set; }

        /// <summary>
        /// 凭证号
        /// <summary>
        
        public string CertificateCode { get; set; }

        /// <summary>
        /// 数据行状态
        /// <summary>
        
        public TState CurState { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public int? iPrintCount { get; set; }

        /// <summary>
        /// 
        /// <summary>
        
        public bool isElectronic { get; set; }
    }
}

