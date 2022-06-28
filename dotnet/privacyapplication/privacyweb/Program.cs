
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using privacyweb;
using privacyweb.Helpers;
using privacyweb.Interface;
using privacyweb.Resources;
using privacyweb.Service;
using privacyweb.Settings;
using System.Configuration;
//https://wiki.sunet.se/display/SWAMID/SWAMID+Entity+Category+Release+Check+-+Privacy+Policy?showLanguage=en_GB
//
//https://md.nordu.net/entities
var builder = WebApplication.CreateBuilder(args);

//LocService
builder.Services.AddSingleton<LocService>();

//LanguageSettings
var languageSettings = new LanguageSettings();
builder.Configuration.GetSection("LanguageSettings").Bind(languageSettings);
builder.Services.AddSingleton<LanguageSettings>(languageSettings);
//PrivacySettings
var privacySettings = new PrivacySettings();
builder.Configuration.GetSection("PrivacySettings").Bind(privacySettings);
foreach (var f in privacySettings.CultureFiles)
{
    privacySettings.PrivacyContent.Add(FilePathHelper.GetPrivacyTexts(f));
}
builder.Services.AddSingleton<PrivacySettings>(privacySettings);

//AttributesSetting
var attributeSettings = new AttributeSettings();
builder.Configuration.GetSection("AttributeSettings").Bind(attributeSettings);
foreach (var f in attributeSettings.CultureFiles)
{
    attributeSettings.Attributes.Add(FilePathHelper.GetAttributeTexts(f));
}
builder.Services.AddSingleton<AttributeSettings>(attributeSettings);


//InformationSettings
var informationSettings = new InformationSettings();
builder.Configuration.GetSection("InformationSettings").Bind(informationSettings);
foreach (var f in informationSettings.CultureFiles)
{
    informationSettings.Informations.AddRange(FilePathHelper.GetInformationTexts(f));
}
builder.Services.AddSingleton<InformationSettings>(informationSettings);

IMDQService mdqservice = new MDQService(builder.Configuration.GetValue<string>("MDQEndpoint"));
builder.Services.AddSingleton<IMDQService>(mdqservice);

// 1. 
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
// 2.
builder.Services.AddControllersWithViews()
    .AddViewLocalization
    (LanguageViewLocationExpanderFormat.SubFolder)
    .AddDataAnnotationsLocalization();
// 3. 
builder.Services.Configure<RequestLocalizationOptions>(options => {
    var supportedCultures = new[] { "en", "sv" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 4. 
var supportedCultures = new[] { "en", "sv"};
// 5. 
// Culture from the HttpRequest
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}");

});
app.Run();
