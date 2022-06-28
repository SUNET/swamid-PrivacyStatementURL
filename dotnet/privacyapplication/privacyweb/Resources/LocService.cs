using Microsoft.Extensions.Localization;
using System.Reflection;

namespace privacyweb.Resources
{
    public class LocService
    {
        private readonly IStringLocalizer _localizer;

        public LocService(IStringLocalizerFactory factory)
        {
            var type = typeof(privacyweb.SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString Get(string key)
        {
            var item = _localizer[key];
            return item;
        }
    }
}
