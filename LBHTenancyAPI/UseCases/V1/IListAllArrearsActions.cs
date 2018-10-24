namespace LBHTenancyAPI.UseCases.V1
{
    public interface IListAllArrearsActions
    {
        ListAllArrearsActions.ArrearsActionDiaryResponse Execute(string tenancyRef);
    }
}
