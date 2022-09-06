using privacyweb.Interface;
using privacyweb.Model;
using System.Net;
using System.Web;
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
                    var nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("samla", "urn:oasis:names:tc:SAML:2.0:assertion");
                    nsmgr.AddNamespace("mdattr", "urn:oasis:names:tc:SAML:metadata:attribute");
                    nsmgr.AddNamespace("mdui", "urn:oasis:names:tc:SAML:metadata:ui");
                    //nsmgr.AddNamespace("pyff", "http://pyff.io/NS");
                    //nsmgr.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                    //nsmgr.AddNamespace("shibmd", "urn:mace:shibboleth:metadata:1.0");
                    //nsmgr.AddNamespace("http://docs.oasis-open.org/ns/xri/xrd-1.0", "urn:mace:shibboleth:metadata:1.0");
                    //nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
                    //nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    //nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

                    //Entity Categories
                    var cats = doc.SelectNodes(@"//mdattr:EntityAttributes/samla:Attribute[@Name='http://macedir.org/entity-category']/samla:AttributeValue",nsmgr);
                    foreach (XmlNode cat in cats)
                    {
                        switch (cat.InnerText.ToLower())
                        {
                            case "http://refeds.org/category/research-and-scholarship":
                                ret.AddAttribute("eduPersonTargetedID");
                                ret.AddAttribute("eduPersonPrincipalName");
                                ret.AddAttribute("mail");
                                ret.AddAttribute("givenName");
                                ret.AddAttribute("sn");
                                ret.AddAttribute("eduPersonAssurance");
                                ret.AddAttribute("eduPersonScopedAffiliation");
                                break;
                            case "https://refeds.org/category/personalized":
                                ret.AddAttribute("subject-id");
                                ret.AddAttribute("mail");
                                ret.AddAttribute("displayName");
                                ret.AddAttribute("givenName");
                                ret.AddAttribute("sn");
                                ret.AddAttribute("eduPersonAssurance");
                                ret.AddAttribute("eduPersonScopedAffiliation");
                                ret.AddAttribute("schacHomeOrganization");
                                break;
                            default:
                                break;
                        }
                    }

                    //Attributes
                    var nodes = doc.GetElementsByTagName("md:RequestedAttribute");
                    foreach (XmlNode node in nodes)
                    {
                        ret.Attributes.Add(node.Attributes.GetNamedItem("FriendlyName").Value);
                    }

                    //Names
                    nodes = doc.GetElementsByTagName("md:ServiceName");
                    if(nodes.Count > 0)
                    {
                        foreach (XmlNode node in nodes)
                        {
                            ret.SystemNames.Add(new SystemName()
                            {
                                Name = node.InnerText,
                                Lang = node.Attributes.GetNamedItem("xml:lang").Value
                            });
                        }
                    }
                    else
                    {
                        ret.SystemNames.Add(new SystemName()
                        {
                            Name =  HttpUtility.UrlDecode( entityId),
                            Lang="en"
                        });
                        ret.SystemNames.Add(new SystemName()
                        {
                            Name = entityId,
                            Lang = "sv"
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
