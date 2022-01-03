using CloudPharmacy.Patient.WebApp.Infrastructure.API;
using CloudPharmacy.Patient.WebApp.Infrastructure.Configuration;
using CloudPharmacy.Patient.WebApp.Infrastructure.Notifications;
using CloudPharmacy.Patient.WebApp.Utils;
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

builder.Services.Configure<PatientAPIConfiguration>(builder.Configuration.GetSection("PatientAPIConfiguration"));
builder.Services.AddSingleton<IValidateOptions<PatientAPIConfiguration>, PatientAPIConfigurationValidation>();
var patientAPIConfiguration = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<PatientAPIConfiguration>>().Value;
builder.Services.AddSingleton<IPatientAPIConfiguration>(patientAPIConfiguration);
builder.Services.AddHttpClient<IPatientAPI, PatientAPI>();

builder.Services.Configure<PhysicianAPIConfiguration>(builder.Configuration.GetSection("PhysicianAPIConfiguration"));
builder.Services.AddSingleton<IValidateOptions<PhysicianAPIConfiguration>, PhysicianAPIConfigurationValidation>();
var physicianAPIConfiguration = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<PhysicianAPIConfiguration>>().Value;
builder.Services.AddSingleton<IPhysicianAPIConfiguration>(physicianAPIConfiguration);
builder.Services.AddHttpClient<IPhysicianAPI, PhysicianAPI>();

builder.Services.AddSingleton<VerifiableCredentialsNotificationService>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddSingleton(typeof(PageParameterService<>));

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
