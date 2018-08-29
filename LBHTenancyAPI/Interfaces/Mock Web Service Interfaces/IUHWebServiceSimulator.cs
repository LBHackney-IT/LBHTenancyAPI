using System;
using System.Threading.Tasks;
using ArrearsAgreementService;

namespace LBHTenancyAPI.Interfaces
{
    public interface IUHWebServiceSimulator
    {
        // Create the interface accessable classes / methods
        Task<ArrearsActionDiaryResponse> CreateArrearsActionDiaryEntryAsync(NewArrearsActionDiaryEntry entry);
    }
}
