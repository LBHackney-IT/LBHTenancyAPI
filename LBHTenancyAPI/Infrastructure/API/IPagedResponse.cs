namespace LBHTenancyAPI.Infrastructure.API
{
    public interface IPagedResponse
    {
        int PageCount { get; set; }
        int TotalCount { get; set; }
    }
}