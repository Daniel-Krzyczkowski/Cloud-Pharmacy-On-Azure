using CloudPharmacy.PharmacyStore.WebApp.Infrastructure.API;
using CloudPharmacy.PharmacyStore.WebApp.Infrastructure.Configuration;
using CloudPharmacy.PharmacyStore.WebApp.Infrastructure.Notifications;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2CConfiguration"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.Configure<VerifiableCredentialsNotificationServiceConfiguration>(builder.Configuration.GetSection("VerifiableCredentialsNotificationServiceConfiguration"));
builder.Services.AddSingleton<IValidateOptions<VerifiableCredentialsNotificationServiceConfiguration>, VerifiableCredentialsNotificationServiceConfigurationValidation>();
var verifiableCredentialsNotificationServiceConfiguration = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<VerifiableCredentialsNotificationServiceConfiguration>>().Value;
builder.Services.AddSingleton<IVerifiableCredentialsNotificationServiceConfiguration>(verifiableCredentialsNotificationServiceConfiguration);


builder.Services.Configure<PharmacyStoreAPIConfiguration>(builder.Configuration.GetSection("PharmacyStoreAPIConfiguration"));
builder.Services.AddSingleton<IValidateOptions<PharmacyStoreAPIConfiguration>, PharmacyStoreAPIConfigurationValidation>();
var pharmacyStoreAPIConfiguration = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<PharmacyStoreAPIConfiguration>>().Value;
builder.Services.AddSingleton<IPharmacyStoreAPIConfiguration>(pharmacyStoreAPIConfiguration);
builder.Services.AddHttpClient<IPharmacyStoreAPI, PharmacyStoreAPI>();

builder.Services.AddSingleton<VerifiableCredentialsNotificationService>();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
