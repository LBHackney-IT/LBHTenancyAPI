namespace LBHTenancyAPI.Infrastructure.V2.API
{
    public interface IPagedRequest
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}
