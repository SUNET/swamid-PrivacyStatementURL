using privacyweb.Model;

namespace privacyweb.Settings
{
    public class InformationSettings
    {
        public InformationSettings()
        {
            Informations = new List<Infolist>(2);
        }
        public string DefaultCulture { get; set; }

        public List<string> SupportedCultures { get; set; }
        public List<string> CultureFiles { get; set; }
        public List<Infolist> Informations { get; set; }
        public string InfoHeader_sv { get; set; }
        public string InfoHeader_en { get; set; }
    }
    public class Info
    {
        public List<Infolist> Infolist { get; set; }
    }
    public class Infolist
    {
        public List<string> entityids { get; set; }
        public string lang { get; set; }
        public Information information { get; set; }
    }

    public class Information
    {
        public string title { get; set; }
        public List<string> text { get; set; }
    }
   
}
