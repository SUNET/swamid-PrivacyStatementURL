namespace privacyweb.Settings
{
    public class LanguageSettings
    {
        public LanguageSettings()
        {

        }
        public string DefaultCulture { get; set; }
        public string[] SupportedCultures { get; set; }
        public List<NavSetting> NavSettings { get; set; }
        public string? SubsiteName { get; set; }
    }
    public class NavSetting
    {
        public string lang { get; set; }
        public string navText { get; set; }
        public string langFlag { get; set; }
    }

}
