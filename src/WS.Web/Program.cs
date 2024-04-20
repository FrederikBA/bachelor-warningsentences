using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Integration.Configuration;
using WS.Core.Interfaces.DomainServices;
using WS.Core.Interfaces.Repositories;
using WS.Core.Services;
using WS.Infrastructure.Data;
using WS.Web.Interfaces;
using WS.Web.Services;

const string policyName = "AllowOrigin";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

//DBContext
builder.Services.AddDbContext<WarningSentenceContext>(options =>
{
    options.UseSqlServer(Constants.ConnectionStrings.ShwWarningSentences);
});

//Build repositories
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));


//Build services
builder.Services.AddScoped<IWarningSentenceService, WarningSentenceService>();

builder.Services.AddScoped<IWarningSentenceViewModelService, WarningSentenceViewModelService>();

//JWT Key
var key = Encoding.UTF8.GetBytes(Constants.Authorization.JwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireShippingCompanyAdminRole", policy => policy.RequireRole("ShippingCompanyAdmin"));
    options.AddPolicy("RequireKemiDbUserRole", policy => policy.RequireRole("KemiDbUser"));
    options.AddPolicy("RequireSuperAdminRole", policy => policy.RequireRole("SuperAdmin"));

    options.AddPolicy("IntegrationPolicy", policy =>
        policy.RequireClaim("service_integration_policy", "service_integration_policy"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(policyName);
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();