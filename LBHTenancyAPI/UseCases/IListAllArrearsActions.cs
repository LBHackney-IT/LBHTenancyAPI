using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases
{
    public interface IListAllArrearsActions
    {
        ListAllArrearsActions.ArrearsActionDiaryResponse Execute(string tenancyRef);
    }
}
