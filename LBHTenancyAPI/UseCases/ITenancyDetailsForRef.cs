namespace LBHTenancyAPI.UseCases
{
     public interface ITenancyDetailsForRef
     {
            TenancyDetailsForRef.TenancyResponse Execute(string tenancyRef);
     }

}
