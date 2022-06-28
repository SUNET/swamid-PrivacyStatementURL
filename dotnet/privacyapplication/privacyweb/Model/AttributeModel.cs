//using System.Text.Json.Serialization;

using System.Text.Json.Serialization;

namespace privacyweb.Model
{
    public class AttributeModel
    {
        public List<Attrlist> attrlist { get; set; }
    }

    public class Attrlist
    {
        public List<string> entityId { get; set; }
        public string lang { get; set; }
        public List<Attribute> attributes { get; set; }
    }
    public class Attribute
    {
        public string name { get; set; }
        public string personaldata { get; set; }
        public string purpose { get; set; }
    }

   

}
