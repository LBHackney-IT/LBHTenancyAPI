namespace LBHTenancyAPI.Infrastructure.V2.API
{
    public interface IPagedResponse
    {
        int PageCount { get; set; }
        int TotalCount { get; set; }
    }
}
