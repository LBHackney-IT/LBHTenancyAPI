using System.Collections.Generic;

namespace LBH.Data.Domain
{
    public class PagedResults<T> : IPagedResults<T>
    {
        public List<T> Results { get; set; }
        public int TotalResultsCount { get; set; }

        public PagedResults()
        {
            Results = new List<T>();
        }

        public int CalculatePageCount(int pageSize)
        {
            if (TotalResultsCount == 0)
                return 0;
            //eg 100 / 10 = 10
            if (TotalResultsCount % pageSize == 0)
                return TotalResultsCount / pageSize;
            //eg 101 / 10 = 10.1 so we cast to 10 and add 1 (11)
            return (int) (TotalResultsCount / pageSize) + 1;
        }
    }
}
