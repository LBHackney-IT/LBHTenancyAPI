namespace LBHTenancyAPI.UseCases.V1
{
     public interface ITenancyDetailsForRef
     {
            TenancyDetailsForRef.TenancyResponse Execute(string tenancyRef);
     }
}
