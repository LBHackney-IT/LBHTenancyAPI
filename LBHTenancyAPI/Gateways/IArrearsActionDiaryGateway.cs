using System;
using AgreementService;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Gateways
{
    public interface IArrearsActionDiaryGateway
    {
        Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ArrearsActionCreateRequest request);
    }
}
