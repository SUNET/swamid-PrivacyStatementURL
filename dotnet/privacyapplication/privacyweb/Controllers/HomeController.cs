using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using privacyweb.Common;
using privacyweb.Interface;
using privacyweb.Model;
using privacyweb.Resources;
using privacyweb.Settings;
using System.Web;

namespace privacyweb.Controllers
{
    public class HomeController : BaseController
    {
        private PrivacySettings _privacySettings;
        private AttributeSettings _attributeSettings;
        
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMDQService _service;
        public HomeController(IStringLocalizer<SharedResource> localizer, PrivacySettings privacySettings,
            AttributeSettings attributeSettings,LanguageSettings languageSettings, IMDQService service) : base(languageSettings)
        {
            _localizer = localizer;
            _privacySettings = privacySettings;
            _attributeSettings = attributeSettings;
            _service = service;
        }
        public IActionResult Index([FromQuery] Query query)
        {
            var systemName = "";
            SetLanguageFromQuery(query);
            PrivacyModel p = new PrivacyModel();
            var empty =  GetEmpty();
            if (string.IsNullOrEmpty(query.system)){
                return View("Error",empty);
            }
            else
            {
                var pl = (from l in _privacySettings.PrivacyContent.SelectMany(e => e.privacylist.Where(p => p.entityId.Any(y => y.Contains("default")) && p.lang == _culture)) select l).ToList();
                var sl = (from l in _privacySettings.PrivacyContent.SelectMany(e => e.privacylist.Where(p => p.entityId.Any(y => y.Contains(query.system.ToLower())) && p.lang == _culture)) select l).ToList();
                //Get Attributes from MDQ
                var mdqModel = _service.GetRequestedAttributes(HttpUtility.UrlEncode(query.system));
                if (mdqModel != null)
                {
                    if (mdqModel.SystemNames.Count>0)
                    {
                        systemName = (from s in mdqModel.SystemNames.Where(l => l.Lang == _culture) select s.Name).SingleOrDefault();
                    }
                    else {
                        systemName = query.system; 
                    }

                    if (sl !=null && sl.Count>0)
                    {
                        if (sl.First().sections == null) {
                            sl.First().sections = new List<section>();
                        }
                        p.privacylist =sl;
                        //Add missing sections if special konfiguration
                        foreach(var section in pl.First().sections)
                        {        
                            if (!sl.First().sections.Any(t => t.id == section.id))
                            {
                                p.privacylist.First().sections.Add(section);
                            }
                        }
                        SetTitle(p, systemName);
                        SetAttributes(p, mdqModel, query.system);
                        p.Navigations= GetLangNavigation();
                        p.System = query.system;
                        return View(p);
                    }
                    else
                    {
                        p.privacylist = pl;
                        SetTitle(p, systemName);
                        SetAttributes(p, mdqModel, query.system);
                        p.Navigations = GetLangNavigation();
                        p.System = query.system;
                        return View(p);
                    }
                }
                else { return View("Error", empty); }
            }
        }

        private void SetAttributes(PrivacyModel pm, MDQModel m,string systemName)
        {
            pm.privacylist[0].sections[1].subsections[0].attributes.properties.Clear();
            foreach (var a in m.Attributes)
            {
                pm.privacylist[0].sections[1].subsections[0].attributes.properties.Add(GetAttribute(a, systemName));
            }
        }

        private void SetTitle(PrivacyModel pm, string systemName)
        {
            pm.privacylist.First().title=pm.privacylist.First().title.Replace("#SYSTEM", systemName);
            foreach (var s in pm.privacylist.First().sections) { 
                s.title=s.title.Replace("#SYSTEM", systemName);
            }
        }

        private void SetLanguageFromQuery(Query query)
        {
            _culture = _privacySettings.DefaultCulture.ToLower();
            if (query.lang != null)
            {
                if (_privacySettings.SupportedCultures.Contains(query.lang.ToLower()))
                {
                    _culture = query.lang.ToLower();
                    Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(query.lang)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) }
                    );
                }
            }
            else
            {
                if (_privacySettings.SupportedCultures.Contains(System.Globalization.CultureInfo.CurrentCulture.Name))
                {
                    _culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                }

            }
            query.lang = null;
        }

        private requiredproperty GetAttribute(string name, string entityId)
        {
            //Check for entityId and attributename
            var l = _attributeSettings.Attributes;
            var attribute = (from x in l.SelectMany(p => p.attrlist.Where(ll => ll.lang == _culture && ll.entityId.Contains(entityId) && ll.attributes.Any(aa=>aa.name.ToLower()==name.ToLower()))) select x.attributes).ToList();
            if (attribute.Count==0)
            {
                // Get from default list
                attribute = (from x in l.SelectMany(p => p.attrlist.Where(ll => ll.lang == _culture && ll.entityId.Contains("default")))select x.attributes).ToList();
            }
            var rp = attribute.First().Where(c => c.name.ToLower() == name.ToLower()).FirstOrDefault();
            return new requiredproperty()
            {
                attribute = HttpUtility.HtmlEncode(rp.personaldata),
                purpose =HttpUtility.HtmlEncode(rp.purpose),
                technicalrepresentation = HttpUtility.HtmlEncode(rp.name)
            };
        }
    }
}
