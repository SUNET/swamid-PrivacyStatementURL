using privacyweb.Settings;

namespace privacyweb.Model
{
    public class InformationModel
    {
        public string SystemName { get; set; }
        public string System { get; set; }
        public Information information { get; set; }
        public List<Navigation> Navigations { get; set; }
    }

   
}
