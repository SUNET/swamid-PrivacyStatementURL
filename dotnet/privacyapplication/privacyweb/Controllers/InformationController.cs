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
    public class InformationController : BaseController
    {
        private InformationSettings _informationSettings;
        private readonly IMDQService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public InformationController(IStringLocalizer<SharedResource> localizer,
            InformationSettings informationSettings, LanguageSettings languageSettings
            , IMDQService service):base(languageSettings)
        {
            _localizer = localizer;
            _informationSettings = informationSettings;
            _service = service;
        }
        public IActionResult Index([FromQuery] Query query)
        {
            var systemName = "";
            SetPrivacyLanguage(query);
            
            var empty = GetEmpty();
            if (string.IsNullOrEmpty(query.system))
            {
                return View("Error", empty);
            }
            else
            {
                var model = new InformationModel();
                model.Navigations = GetLangNavigation();
                var info = (from i in _informationSettings.Informations.Where(s => s.lang == _culture && s.entityids.Contains(query.system)) select i).FirstOrDefault();
                var mdqModel = _service.GetRequestedAttributes(HttpUtility.UrlEncode(query.system));

                if (info != null && mdqModel!=null) {
                    model.System = (from s in mdqModel.SystemNames.Where(l => l.Lang == _culture) select s.Name).SingleOrDefault();
                    model.SystemName =_localizer.GetString("InformationTitle") + " " + model.System;
                    model.information = info.information;
                    return View(model);
                }
                else { return View("Error", empty); }
            }
        }
        private void SetPrivacyLanguage(Query query)
        {
            _culture = _informationSettings.DefaultCulture.ToLower();
            if (query.lang != null)
            {
                if (_informationSettings.SupportedCultures.Contains(query.lang.ToLower()))
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
                if (_informationSettings.SupportedCultures.Contains(System.Globalization.CultureInfo.CurrentCulture.Name))
                {
                    _culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                }

            }
            query.lang = null;
        }
    }
}
