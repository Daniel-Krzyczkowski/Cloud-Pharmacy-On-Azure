using CloudPharmacy.Common.ExceptionMiddleware;
using CloudPharmacy.VerifiableCredentials.API.Core.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppConfiguration(builder.Configuration);
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer("ClientCredentialsScheme", jwtOptions =>
        {
            jwtOptions.MetadataAddress = builder.Configuration["AzureAdB2CConfiguration:MetadataAddress"];
            jwtOptions.Authority = builder.Configuration["AzureAdB2CConfiguration:Authority"];
            jwtOptions.Audience = builder.Configuration["AzureAdB2CConfiguration:ClientId"];
        });

builder.Services.AddAuthorization(options =>
{
    var authorizationPolicy = new AuthorizationPolicyBuilder()
                                  .RequireRole("vc.access")
                                  .AddAuthenticationSchemes("ClientCredentialsScheme")
                                  .Build();
    options.AddPolicy("Verifiable-Credentials-Access", authorizationPolicy);
});


builder.Services.AddMemoryCache();

builder.Services.AddVerifiableCredentialsServices();
builder.Services.AddSignalR();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cloud Pharmacy Verifiable Credentials API",
        Description = "API to manage Verifiable Credentials for patients",
        Contact = new OpenApiContact
        {
            Name = "Daniel Krzyczkowski",
            Email = string.Empty,
            Url = new Uri("https://twitter.com/dkrzyczkowski"),
        },
        License = new OpenApiLicense
        {
            Name = "Use under MIT"
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                    }
                });
});

var app = builder.Build();
app.UseMiddleware<ApiExceptionMiddleware>();

//app.UseMiddleware<ApiExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
