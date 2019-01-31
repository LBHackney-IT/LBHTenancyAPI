using System;

namespace LBHTenancyAPI.UseCases.V1
{
    public static class TenancyDateFormatter
    {
        public static string UniversalSortable(DateTime? date)
        {
            return date.HasValue ? string.Format("{0:u}", date) : null;
        }
    }
}
