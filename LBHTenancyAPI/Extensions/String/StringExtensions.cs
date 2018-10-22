using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Extensions.String
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNotNullOrEmptyOrWhiteSpace(this string str)
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        }
    }
}
