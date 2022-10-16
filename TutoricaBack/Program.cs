Console.WriteLine("Hello, everyone! Version 0.0.1");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapGet("/gettest", () =>
{
    return "GET Tested!";
});

app.MapPost("/posttest", () =>
{
    return "POST Tested!";
});

app.Run();
