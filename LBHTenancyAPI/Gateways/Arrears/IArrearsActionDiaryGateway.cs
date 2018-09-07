using System;
using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.Gateways
{
    public interface IArrearsActionDiaryGateway
    {
        Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ArrearsActionCreateRequest request);
    }
}
