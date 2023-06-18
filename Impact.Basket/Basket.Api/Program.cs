using Basket.Api.Security;
using Basket.Contracts;
using Basket.Repositories.Repositories;
using Impact.Connectores;
using Impact.Core.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//Add the Repository service to the services container
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// Register the CodeChallengeConnectorOptions class with the configuration values
builder.Services.Configure<CodeChallengeConnectorOptions>(config.GetSection("ImpactCodeChallengeUrl"));

builder.Services.AddScoped<ICodeChallengeConnector, CodeChallengeConnector>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IIdentityProvider, AspNetCoreClaimsIdentityProvider>();


var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
var keyBytes = Encoding.UTF8.GetBytes(jwtSecretKey);
var signingKey = new SymmetricSecurityKey(keyBytes);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
