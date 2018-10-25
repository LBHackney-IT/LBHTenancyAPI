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

        /// <summary>
        /// Based on 1 based paging not 0 based
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="totalResultsCount"></param>
        /// <returns></returns>
        public int CalculatePageCount(int pageSize, int totalResultsCount)
        {
            if (totalResultsCount == 0)
                return 1;
            //eg 100 / 10 = 10
            if (totalResultsCount % pageSize == 0)
                return totalResultsCount / pageSize;
            //eg 101 / 10 = 10.1 so we cast to 10 and add 1 (11)
            var pageCount = (int) (totalResultsCount / pageSize) + 1;
            if (pageCount == 0)
                pageCount = 1;
            return pageCount;
        }
    }
}
