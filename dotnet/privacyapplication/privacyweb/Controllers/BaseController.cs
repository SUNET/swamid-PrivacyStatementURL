using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using privacyweb.Model;
using privacyweb.Settings;
namespace privacyweb.Controllers
{
    public class BaseController : Controller
    {

        public string _culture;
        public LanguageSettings _languageSettings;
        public BaseController(LanguageSettings languageSettings)
        {
            _languageSettings=languageSettings; 
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) }
            );

            return LocalRedirect(returnUrl);
        }
        
        public Empty GetEmpty()
        {
            var empty = new Empty();
            if (_culture == "sv")
            {
                empty.Text = "Inget EntityID angivet!";
            }
            else
            {
                empty.Text = "No EntityID provided!";
            }
            return empty;
        }
        
        public List<Navigation> GetLangNavigation()
        {
            var navigations = new List<Navigation>(3);
            foreach (var l in _languageSettings.SupportedCultures)
            {
                if (l.ToLower() != _culture)
                {
                    var ls = (from nav in _languageSettings.NavSettings.Where(t => t.lang == l) select nav).SingleOrDefault();
                    if (ls != null)
                    {
                        navigations.Add(new Navigation()
                        {
                            Culture = ls.lang,
                            CultureLogo = GetLogoPath(ls.langFlag),
                            CultureText = ls.navText
                        });
                    }
                }
            }
            return navigations;
        }
        private string GetLogoPath(string path)
        {
            var imgPath = "/images/" + path;
            if (!string.IsNullOrEmpty(_languageSettings.SubsiteName))
            {
                imgPath = "/" + _languageSettings.SubsiteName + imgPath;
            }
            return imgPath;
        }

    }
}
