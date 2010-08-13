namespace Rhino.Licensing.AdminTool.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}