using System;
using LBHTenancyAPI.ArrearsAgreementService;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Interfaces
{
    public interface IArrearsActionDiaryService
    {
        Task<ArrearsActionResponse> CreateArrearsActionAsync(ArrearsActionCreateRequest request);
    }
}
