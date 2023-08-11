var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Built in dependency injection

// Register services that are required for API's
builder.Services.AddControllers();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
