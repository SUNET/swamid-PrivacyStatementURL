namespace privacyweb.Common
{
    public class Query
    {
        private string _lang;
        private string _system;
        public string lang
        {
            get
            {
                return _lang;
            }
            set
            {
                _lang = value;
            }
        }
        public string system
        {
            get { return _system; }
            set { _system = value; }
        }
    }
}
