var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Built in dependency injection

// Register services that are required for API's
builder.Services.AddControllers(
    options => 
        // When requesting value to be in different format ie. `application/xml` it will still return JSON unless this is configured
        // with this the return will be 406 Not Acceptable
        options.ReturnHttpNotAcceptable = true
    )
    // Add support for xml format
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// This will create the web application
// app inherits from `IApplicationBuilder` 
// This enables us to configure the applications request pipelines
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

app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
