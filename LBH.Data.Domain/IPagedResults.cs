using System.Collections.Generic;

namespace LBH.Data.Domain
{
    public interface IPagedResults<T>
    {
        List<T> Results { get; set; }
        int TotalResultsCount { get; set; }
    }
}
