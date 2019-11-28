using System;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;

namespace LBHTenancyAPI.Gateways.V2.Arrears
{
    public interface IArrearsActionDiaryGateway
    {
        Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ActionDiaryRequest request);
    }
}
