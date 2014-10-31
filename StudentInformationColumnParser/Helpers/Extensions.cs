namespace ParserUtilities.Helpers
{
    public static class Extensions
    {
        public static string Append(this string source, string addition)
        {
            return string.Format("{0}{1}", source, addition);
        }
    }
}
