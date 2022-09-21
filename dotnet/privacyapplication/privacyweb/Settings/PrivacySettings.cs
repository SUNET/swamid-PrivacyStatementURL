

using privacyweb.Model;

namespace privacyweb.Settings
{
    public class PrivacySettings
    {
        public PrivacySettings()
        {
            PrivacyContent = new List<PrivacyModel>(3);
        }
        public string SubsiteName { get; set; }
        public string DefaultCulture { get; set; }

        public List<string> SupportedCultures { get; set; }

        public List<string> CultureFiles { get; set; }
        public List<PrivacyModel> PrivacyContent { get; set; }
        public string SystemName_sv { get; set; }
        public string SystemName_en { get; set; }
        public string EmptyAttributes_sv { get; set; }
        public string EmptyAttributes_en { get; set; }
    }
}
