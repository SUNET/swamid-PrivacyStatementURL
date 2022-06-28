using privacyweb.Interface;
using privacyweb.Model;
using System.Net;
using System.Xml;

namespace privacyweb.Service
{
    public class MDQService : IMDQService
    {
        public string MDQUri { get; set; }
        public MDQService(string uri)
        {
            MDQUri = uri;   
        }

        public MDQModel GetRequestedAttributes(string entityId)
        {
            var ret = new MDQModel();
            using (var client =GetClient())
            {
                try
                {
                    var xml = client.DownloadString(entityId);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    //var nsmgr = new XmlNamespaceManager(doc.NameTable);
                    //nsmgr.AddNamespace("md", "urn:oasis:names:tc:SAML:2.0:metadata");
                    //nsmgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                    //nsmgr.AddNamespace("mdattr", "urn:oasis:names:tc:SAML:metadata:attribute");
                    //nsmgr.AddNamespace("mdrpi", "urn:oasis:names:tc:SAML:metadata:rpi");
                    //nsmgr.AddNamespace("mdui", "urn:oasis:names:tc:SAML:metadata:ui");
                    //nsmgr.AddNamespace("pyff", "http://pyff.io/NS");
                    //nsmgr.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                    //nsmgr.AddNamespace("shibmd", "urn:mace:shibboleth:metadata:1.0");
                    //nsmgr.AddNamespace("http://docs.oasis-open.org/ns/xri/xrd-1.0", "urn:mace:shibboleth:metadata:1.0");
                    //nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
                    //nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    //nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
                    //XmlElement xmlElem = doc.DocumentElement;
                    //XmlNodeList nodeList = xmlElem.SelectNodes(".//md:RequestedAttribute");

                    //Attributes
                    var nodes = doc.GetElementsByTagName("md:RequestedAttribute");
                    foreach (XmlNode node in nodes)
                    {
                        ret.Attributes.Add(node.Attributes.GetNamedItem("FriendlyName").Value);
                    }
                    //Names
                    nodes = doc.GetElementsByTagName("md:ServiceName");
                    foreach (XmlNode node in nodes)
                    {
                        ret.SystemNames.Add(new SystemName()
                        {
                            Name = node.InnerText,
                            Lang = node.Attributes.GetNamedItem("xml:lang").Value
                        });
                    }
                }catch (Exception ex)
                {
                    ret = null;
                }
            }
            return ret;
        }
        private WebClient GetClient()
        {
            var client = new WebClient();
            //client.BaseAddress = string.Join("/", MDQUri, entityId);
            client.BaseAddress = MDQUri+"/";
            return client;
        }
    }
}
