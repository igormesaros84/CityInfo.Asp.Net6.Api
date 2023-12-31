using CityInfo.Api;
using CityInfo.Api.DbContexts;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt" , rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// User serilog
builder.Host.UseSerilog();
// Add services to the container.
// Built in dependency injection

// Register services that are required for API's
builder.Services.AddControllers(
    options => 
        // When requesting value to be in different format ie. `application/xml` it will still return JSON unless this is configured
        // with this the return will be 406 Not Acceptable
        options.ReturnHttpNotAcceptable = true
    ).AddNewtonsoftJson()
    // Add support for xml format
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Can be used to dynamically set the content type when downloading files based on the extension
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>();

// SqlLiteDb connection string is just as simple as this
// other providers might have different connection strings that can be found in the documentation
builder.Services.AddDbContext<CityInfoContext>(options =>
    options.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDbConnectionString"]));
// This will create the web application
// app inherits from `IApplicationBuilder` 
// This enables us to configure the applications request pipelines
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

// Setting up the authentication Middleware
builder.Services.AddAuthentication("Bearer").AddJwtBearer(
    options => 
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };

    });

builder.Services.AddAuthorization(option => 
    option.AddPolicy("MustBeeFromAntwerp", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Antwerp");
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
// Bellow are the configurations for different middlewares (the applications request pipeline)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Add the authentication middleware to the request pipeline
app.UseAuthentication();
// Order matters so we want to make sure we are authenticated before we Authorize
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
