using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

Console.WriteLine("Hello, everyone! Version 0.0.2");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("database_configuration.json").Build();

string connectionString = "Server=" + config.GetValue<string>("dbhost") + ";" +
                            "Database=" + config.GetValue<string>("dbname") + ";" +
                            "Trusted_Connection=False;" + 
                            "User Id=" + config.GetValue<string>("dbuser") + ";" +
                            "Password=" + config.GetValue<string>("dbpass") + ";";

System.Console.WriteLine(connectionString);

var connection = new SqlConnection(connectionString);
connection.Open();


app.MapGet("/enterprises", () =>
{
    string sql = "SELECT * FROM [dbo].[items]";
    using var cmd = new SqlCommand(sql, connection);

    using SqlDataReader rdr = cmd.ExecuteReader();

    string resultString = "";

    while (rdr.Read())
    {
        resultString += rdr.GetInt32(0).ToString() + ";" +
                        rdr.GetString(1) + ";" + 
                        rdr.GetString(2) + "\n";

    }
    
    return resultString;


});

app.MapGet("/getcourse", () =>
{
    string sql = "SELECT * FROM [dbo].[items]";
    using var cmd = new SqlCommand(sql, connection);

    using SqlDataReader rdr = cmd.ExecuteReader();

    string resultString = "";

    while (rdr.Read())
    {
        resultString += rdr.GetInt32(0).ToString() + ";" +
                        rdr.GetString(1) + ";" + 
                        rdr.GetString(2) + "\n";

    }
    
    return resultString;


});

app.MapPost("/advertisement", () =>
{
    return "Method is under contstruction";
});

app.Run();

app.MapPost("/feedback", () =>
{
    return "Method is under contstruction";
});

app.Run();
