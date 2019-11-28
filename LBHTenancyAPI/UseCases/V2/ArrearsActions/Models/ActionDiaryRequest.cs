using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LBHTenancyAPI.UseCases.V2.ArrearsActions.Models
{
    public class ActionDiaryRequest
    {
        //For ArrearsAction object
        public string ActionCategory { get; set; }
        [Required] public string ActionCode { get; set; }
        [Required] public string Comment { get; set; }
        [Required] public string TenancyAgreementRef { get; set; }
        public string Username { get; set; }
        public DateTime? CreatedDate { get; set; }

        //For rest of request object
        public string CompanyCode { get; set; }
        public string SessionToken { get; set; }
    }
}
