namespace LBHTenancyAPI.Extensions.String
{
    /// <summary>
    /// Extension class to help with common string validation
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Confirm string does not have any text
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }
        /// <summary>
        /// Confirm we have a string with some text
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmptyOrWhiteSpace(this string str)
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        }
    }
}
