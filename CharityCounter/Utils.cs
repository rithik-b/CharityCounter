namespace CharityCounter
{
    public static class Utils
    {
        public const string DollarsFormat = "{dollars}";
        public const string MissesFormat = "{misses}";
        public const string FailsFormat = "{fails}";

        public static string FormatOutput(string content, float dollars, int notesMissed, int mapsFailed)
        {
            content = content.Replace(DollarsFormat, $"{dollars}");
            content = content.Replace(MissesFormat, $"{notesMissed}");
            content = content.Replace(FailsFormat, $"{mapsFailed}");
            return content;
        }
    }
}
