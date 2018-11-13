using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHTenancyAPI.UseCases.V2.ArrearsActions.Models
{
    public class ActionDiaryRequest
    {
        //For ArrearsAction object
        public decimal ActionBalance { get; set; }
        public string ActionCategory { get; set; }
        public string ActionCode { get; set; }
        public string Comment { get; set; }
        public string TenancyAgreementRef { get; set; }
        //For rest of request object
        public string CompanyCode { get; set; }
        public string  AppUser { get; set; }
        public string SessionToken { get; set; }
    }
}
