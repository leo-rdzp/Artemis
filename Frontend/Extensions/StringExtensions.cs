namespace Artemis.Frontend.Extensions
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString()));
        }
    }
}
