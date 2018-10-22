namespace LBHTenancyAPI.Infrastructure.API
{
    public interface IPagedRequest
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}