namespace CharityCounter
{
    public static class Utils
    {
        public const string DollarsFormat = "{dollars}";
        public const string MissesFormat = "{misses}";
        public const string FailsFormat = "{fails}";

        public const string Numpad =
            @"        
            (77) (88) (99) [<--]/14,22
            (44) (55) (66)
            (11) (22) (33) [ENTER]/14,24
            [0]/22 [.]
            ";
        public static string FormatOutput(string content, float dollars, int notesMissed, int mapsFailed)
        {
            content = content.Replace(DollarsFormat, $"{dollars}");
            content = content.Replace(MissesFormat, $"{notesMissed}");
            content = content.Replace(FailsFormat, $"{mapsFailed}");
            return content;
        }
    }
}
