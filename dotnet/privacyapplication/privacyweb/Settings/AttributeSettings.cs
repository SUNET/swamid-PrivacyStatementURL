using privacyweb.Model;

namespace privacyweb.Settings
{
    public class AttributeSettings
    {
        public AttributeSettings()
        {
            Attributes = new List<AttributeModel>(2);
        }
        public string DefaultCulture { get; set; }

        public List<string> SupportedCultures { get; set; }
        public List<string> CultureFiles { get; set; }
        public List<AttributeModel> Attributes { get; set; }

        

       

    }

}
