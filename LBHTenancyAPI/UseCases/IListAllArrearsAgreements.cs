namespace LBHTenancyAPI.UseCases
{
    public interface IListAllArrearsAgreements
    {
            ListAllArrearsAgreements.ArrearsAgreementResponse Execute(string tenancyRef);
    }
}
