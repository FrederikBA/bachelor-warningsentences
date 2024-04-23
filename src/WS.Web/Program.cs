using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Integration.Configuration;
using WS.Core.Interfaces.DomainServices;
using WS.Core.Interfaces.Integration;
using WS.Core.Interfaces.Repositories;
using WS.Core.Services.DomainServices;
using WS.Core.Services.IntegrationServices;
using WS.Infrastructure.Data;
using WS.Infrastructure.Producers;
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
    options.UseSqlServer(Config.ConnectionStrings.ShwWarningSentences);
});

//Build repositories
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

//Build Kafka producers
builder.Services.AddScoped<IKafkaProducer, SyncProducer>();


//Build services
builder.Services.AddScoped<IWarningSentenceService, WarningSentenceService>();

builder.Services.AddScoped<IWarningSentenceIntegrationService, WarningSentenceIntegrationService>();

builder.Services.AddScoped<IWarningSentenceViewModelService, WarningSentenceViewModelService>();

//JWT Key
var key = Encoding.UTF8.GetBytes(Config.Authorization.JwtKey);

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
    options.AddPolicy(Config.Authorization.Policies.RequireShippingCompanyAdminRole, policy => policy.RequireRole(Config.Authorization.Roles.ShippingCompanyAdmin));
    options.AddPolicy(Config.Authorization.Policies.RequireKemiDbUserRole, policy => policy.RequireRole(Config.Authorization.Roles.KemiDbUser));
    options.AddPolicy(Config.Authorization.Policies.RequireSuperAdminRole, policy => policy.RequireRole(Config.Authorization.Roles.SuperAdmin));
    options.AddPolicy(Config.Authorization.Policies.RequireIntegrationPolicy, policy => policy.RequireRole(Config.Authorization.Roles.IntegrationPolicy));
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