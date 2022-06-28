namespace privacyweb.Model
{
    public class MDQModel
    {
        public MDQModel()
        {
            Attributes = new List<string>();
            SystemNames = new List<SystemName>();
        }
        public List<string> Attributes { get; set; }
        public List<SystemName> SystemNames { get; set; }
    }
    public class SystemName
    {
        public string Name { get; set; }
        public string Lang { get; set; }
    }
}
