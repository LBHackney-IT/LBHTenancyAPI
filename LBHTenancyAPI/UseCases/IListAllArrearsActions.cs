using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases
{
    public interface IListAllArrearsActions
    {
        AllArrearsActionsForTenancy.ArrearsActionDiaryResponse Execute(string tenancyRef);
    }
}
