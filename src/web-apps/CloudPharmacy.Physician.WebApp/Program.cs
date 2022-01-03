using CloudPharmacy.Physician.WebApp.Infrastructure.API;
using CloudPharmacy.Physician.WebApp.Infrastructure.Configuration;
using CloudPharmacy.Physician.WebApp.Utils;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdConfiguration"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.Configure<PhysicianAPIConfiguration>(builder.Configuration.GetSection("PhysicianAPIConfiguration"));
builder.Services.AddSingleton<IValidateOptions<PhysicianAPIConfiguration>, PhysicianAPIConfigurationValidation>();
var physicianAPIConfiguration = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<PhysicianAPIConfiguration>>().Value;
builder.Services.AddSingleton<IPhysicianAPIConfiguration>(physicianAPIConfiguration);
builder.Services.AddHttpClient<IPhysicianAPI, PhysicianAPI>();

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Physician", configurePolicy =>
    {
        configurePolicy.RequireClaim(ClaimConstants.Role, "physician");
    });
});


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
