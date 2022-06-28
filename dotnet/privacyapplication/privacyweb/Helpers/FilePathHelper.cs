using Newtonsoft.Json;
using privacyweb.Model;
using privacyweb.Settings;
using System.Text;
using System.Text.Json;

namespace privacyweb.Helpers
{
    public static class FilePathHelper
    {
        public static string ToApplicationPath(this string fileName)
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                                .Assembly.GetExecutingAssembly().Location).Replace("file:\\", "");

            return Path.Combine(exePath, fileName);
        }
        public static PrivacyModel GetPrivacyTexts(string fileName)
        {
            var path = ToApplicationPath(fileName);
            var json = System.IO.File.ReadAllText(path, Encoding.UTF8);
            var priv = JsonConvert.DeserializeObject<PrivacyModel>(json);
            return priv;
        }
        public static AttributeModel GetAttributeTexts(string fileName)
        {
            var path = ToApplicationPath(fileName);
            var json = System.IO.File.ReadAllText(path, Encoding.UTF8);
            var attrs = JsonConvert.DeserializeObject<AttributeModel>(json);
            return attrs;
        }
        public static List<Infolist> GetInformationTexts(string fileName)
        {
            var path = ToApplicationPath(fileName);
            var json = System.IO.File.ReadAllText(path, Encoding.UTF8);
            var attrs = JsonConvert.DeserializeObject<Info>(json);

            return attrs.Infolist;
        }
    }
}
