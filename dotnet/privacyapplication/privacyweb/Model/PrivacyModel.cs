using System.Text.Json.Serialization;

namespace privacyweb.Model
{
    
    public class PrivacyModel
    {
        public List<Privacylist> privacylist { get; set; }
        public List<Navigation> Navigations { get; set; }
        public string System { get; set; }
        public PrivacyModel()
        {
            privacylist = new List<Privacylist>(3);
        }
    }
    public class Privacylist
    {
        public Privacylist ShallowCopy()
        {
            var sec = new List<section>();
            sec.AddRange(this.sections.Select(s => s.ShallowCopy()));
            var copy = (Privacylist)this.MemberwiseClone();
            copy.sections = sec;
            return copy;
        }
        [JsonPropertyName("entityId")]
        public string[] entityId { get; set; }
        [JsonPropertyName("lang")]
        public string lang { get; set; }
        [JsonPropertyName("title")]
        public string title { get; set; }
        [JsonPropertyName("sections")]
        public List<section> sections { get; set; }
        
    }
    public class section
    {
        public section ShallowCopy()
        {
            var sub = new List<subsection>();
            if(this.subsections != null) { 
                sub.AddRange(this.subsections.Select(s => s.ShallowCopy()));
            }
            return (section)this.MemberwiseClone();
        }
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("title")]
        public string title { get; set; }
        [JsonPropertyName("text")]
        public string[] text { get; set; }
        [JsonPropertyName("subsections")]
        public List<subsection> subsections { get; set; }
    }
    public class subsection
    {
        public subsection ShallowCopy()
        {
            return (subsection)this.MemberwiseClone();
        }
        [JsonPropertyName("title")]
        public string title { get; set; }
        [JsonPropertyName("text")]
        public string[] text { get; set; }
        [JsonPropertyName("attributes")]
        public attribute attributes { get; set; }
    }
    public class attribute
    {
        [JsonPropertyName("headers")]
        public string[] headers { get; set; }
        [JsonPropertyName("properties")]
        public List<requiredproperty> properties { get; set; }
        [JsonPropertyName("footer")]
        public string Footer { get; set; }
    }
    public class requiredproperty
    {
        [JsonPropertyName("attribute")]
        public string attribute { get; set; }
        [JsonPropertyName("purpose")]
        public string purpose { get; set; }
        [JsonPropertyName("technicalrepresentation")]
        public string technicalrepresentation { get; set; }
    }

}
