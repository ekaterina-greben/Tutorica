using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Data.SQLite;
using Newtonsoft.Json.Linq;
using RestSharp;


string formatLogString(string logevent) {
    return "[" + DateTime.Now + "] " + logevent;
}

Console.WriteLine("API Server Version 0.0.5");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

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

var configSQLite3 = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("database_sqlite3_configuration.json").Build();

string connectionStringSQLite3 = "Data Source=" + 
            configSQLite3.GetValue<string>("dbname") + ";Version=3;New=True;Compress=True;";
	
app.Logger.LogDebug(formatLogString(connectionStringSQLite3));
	
SQLiteConnection sqlite3Connection;
sqlite3Connection = new SQLiteConnection(connectionStringSQLite3);
sqlite3Connection.Open();

var configExternalServices = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("external_services.json").Build();

app.MapGet("/university", (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogInformation(formatLogString("GET /university " + "from " + request.Host));
    string sql = @"SELECT id, firstname, lastname, name, phonenumber, email, description, years, 
                 investment_usd, course, investment_uah FROM [dbo].[items]";
    
    using var cmd = new SqlCommand(sql, connection);

    using SqlDataReader rdr = cmd.ExecuteReader();

    List<ListDictionary> answer = new List<ListDictionary>();

    while (rdr.Read()) 
    {

        ListDictionary list = new ListDictionary();
        list.Add("id", rdr.GetInt32(0).ToString());
        list.Add("firstname", rdr.GetString(1));
        list.Add("lastname", rdr.GetString(2));
        list.Add("name", rdr.GetString(3));
        list.Add("phonenumber", rdr.GetString(4));
        list.Add("email", rdr.GetString(5));
        list.Add("description", rdr.GetString(6));
        list.Add("years", rdr.GetInt32(7).ToString());
        list.Add("investmentUSD", rdr.GetDecimal(8).ToString());
        list.Add("course", rdr.GetDecimal(9).ToString());
        list.Add("investmentUAH", rdr.GetDecimal(10).ToString());

        answer.Add(list);

    }
    
    response.Headers.AccessControlAllowOrigin = "*";
    response.ContentType = "application/json";
    response.StatusCode = 200;

    return answer;
});

app.MapGet("/getcourse", async (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogInformation(formatLogString("GET /getcourse " + "from " + request.Host));
    
    var client = new RestClient(configExternalServices["coursesServiceURL"]);
    var courseRequest = new RestRequest(configExternalServices["path"], Method.Get);
    var queryResult = (await client.ExecuteAsync(courseRequest)).Content;
    
    response.Headers.AccessControlAllowOrigin = "*";
    response.ContentType = "application/json";
    response.StatusCode = 200;
    return queryResult;
});


app.MapPost("/university", async (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogInformation(formatLogString("POST /university " + "from " + request.Host));
    
    string body = "";

    using (var reader = new StreamReader(request.Body, null, true, 1024, true))
        {
            body = await reader.ReadToEndAsync();
        }    

    app.Logger.LogDebug(formatLogString(body));

    JObject bodyJson = JObject.Parse(body);

    string sql = String.Format(@"INSERT INTO [dbo].[items] (firstname, lastname, name, phonenumber, email, 
                    description, years, investment_usd, course, investment_uah) VALUES (
                      '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, {8}, {9})", 
                      bodyJson["firstName"], bodyJson["lastName"],
                      bodyJson["name"], bodyJson["phoneNumber"], bodyJson["email"], 
                      bodyJson["description"], bodyJson["years"], 
                      bodyJson["investmentUSD"], bodyJson["course"], bodyJson["investmentUAH"]);

    app.Logger.LogDebug(formatLogString(sql));

    using var cmd = new SqlCommand(sql, connection);
    cmd.ExecuteNonQuery();
    
    response.Headers.AccessControlAllowOrigin = "*";
    response.StatusCode = 201;
    return "Created";
});


app.MapPost("/feedback", async (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogInformation(formatLogString("POST /feedback " + "from " + request.Host));
    
    string body = "";

    using (var reader = new StreamReader(request.Body, null, true, 1024, true))
        {
            body = await reader.ReadToEndAsync();
        }    
    app.Logger.LogDebug(formatLogString(body));

    JObject bodyJson = JObject.Parse(body);

    var email = bodyJson["email"];
    var msgtext = bodyJson["msgText"];

    var sqlite3Cmd = sqlite3Connection.CreateCommand();

    string sqlite3InsertQuery = @"INSERT INTO feedback (email, msgtext) VALUES ('" + email + 
                                "', '" + msgtext + "')";

    app.Logger.LogDebug(formatLogString(sqlite3InsertQuery));

    sqlite3Cmd.CommandText = sqlite3InsertQuery;
    sqlite3Cmd.ExecuteNonQuery();

    response.Headers.AccessControlAllowOrigin = "*";
    response.StatusCode = 201;

    return "OK";
});

app.MapMethods("/feedback", new[] { "OPTIONS" }, (HttpRequest request, HttpResponse response) => {

    app.Logger.LogDebug(formatLogString("OPTIONS /feedback " + "from " + request.Host));

    response.Headers.AccessControlAllowOrigin = "*";
    response.Headers.AccessControlAllowMethods = "*";
    response.Headers.AccessControlAllowHeaders = "*";
    response.StatusCode = 200;

    return "OK";
});

app.MapMethods("/university", new[] { "OPTIONS" }, (HttpRequest request, HttpResponse response) => {

    app.Logger.LogDebug(formatLogString("OPTIONS /university " + "from " + request.Host));

    response.Headers.AccessControlAllowOrigin = "*";
    response.Headers.AccessControlAllowMethods = "*";
    response.Headers.AccessControlAllowHeaders = "*";
    response.StatusCode = 200;

    return "OK";
});

app.Run();

