using System.Collections.Generic;
using LBHTenancyAPI.UseCases.V1;

namespace LBHTenancyAPI.UseCases
{
    public interface IListAllArrearsActions
    {
        ListAllArrearsActions.ArrearsActionDiaryResponse Execute(string tenancyRef);
    }
}
