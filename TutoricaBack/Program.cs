using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Specialized;

string formatLogString(string logevent) {
    return "[" + DateTime.Now + "] " + logevent;
}

Console.WriteLine("API Server Version 0.0.3");

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

app.Logger.LogDebug(formatLogString(connectionString));

var connection = new SqlConnection(connectionString);
connection.Open();


app.MapGet("/enterprises", (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogInformation(formatLogString("GET /enterprises " + "from " + request.Host));
    string sql = @"SELECT id, firstName, lastName, enterpriseName, phoneNumber, email, entDescription, age, a_t_USD, course, a_t_UAH * FROM [dbo].[items]";
    using var cmd = new SqlCommand(sql, connection);

    using SqlDataReader rdr = cmd.ExecuteReader();

    List<ListDictionary> answer = new List<ListDictionary>();

    while (rdr.Read()) 
    {

        ListDictionary list = new ListDictionary();
        list.Add("id", rdr.GetInt32(0).ToString());
        list.Add("firstName", rdr.GetString(1));
        list.Add("lastName", rdr.GetString(2));
        list.Add("phoneNumber", rdr.GetString(3));
        list.Add("email", rdr.GetString(4));
        list.Add("entDescription", rdr.GetString(5));
        list.Add("age", rdr.GetInt32(6).ToString());
        list.Add("a_t_USD", rdr.GetDecimal(7).ToString());
        list.Add("course", rdr.GetDecimal(8).ToString());
        list.Add("a_t_UAH", rdr.GetDecimal(9).ToString());

        answer.Add(list);

    }
    
    response.Headers.AccessControlAllowOrigin = "*";
    response.StatusCode = 200;

    return answer;
});

app.MapGet("/getcourse", (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogWarning(formatLogString("GET /getcourse " + "from " + request.Host));
    app.Logger.LogInformation(formatLogString("GET /getcourse " + "from " + request.Host));
    string sql = @"SELECT date, currency, course * FROM [dbo].[items]";
    using var cmd = new SqlCommand(sql, connection);

    using SqlDataReader rdr = cmd.ExecuteReader();

    List<ListDictionary> answer = new List<ListDictionary>();

    while (rdr.Read())
    {

        ListDictionary list = new ListDictionary();
        list.Add("date", rdr.GetInt32(0).ToString());
        list.Add("currency", rdr.GetString(1));
        list.Add("course", rdr.GetFloat(2));
        
        answer.Add(list);

    }

    response.Headers.AccessControlAllowOrigin = "*";
    response.StatusCode = 200;

    return answer;
});


app.MapPost("/advertisement", (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogDebug(formatLogString("POST /advertisement " + "from " + request.Host));
    response.Headers.AccessControlAllowOrigin = "*";
    response.StatusCode = 201;
    return "Method is under contstruction";
});

app.Run();

app.MapPost("/feedback", (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogCritical(formatLogString("POST /feedback " + "from " + request.Host));
    app.Logger.LogDebug(formatLogString("POST /feedback " + "from " + request.Host));
    response.Headers.AccessControlAllowOrigin = "*";
    response.StatusCode = 201;
    return "Method is under contstruction";
});

app.Run();
