using System;
using AgreementService;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Interfaces
{
    public interface IArrearsActionDiaryService
    {
        Task<ArrearsActionResponse> CreateActionDiaryRecord(ArrearsActionCreateRequest request);
    }
}
