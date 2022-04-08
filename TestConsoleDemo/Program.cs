// See https://aka.ms/new-console-template for more information
using Lucktian.DbConfigProvider;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Linq;


ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsetting.json");
var configFile = configBuilder.Build();
var connectionString = configFile["DbSetting:ConnectionString"];
configBuilder.AddDbConfiguration(() => new MySqlConnection(connectionString));
var config = configBuilder.Build();
var appitems = config.GetSection("Api:Appitems").Get<ApplicationItem[]>();
string[] strs = config.GetSection("Cors:Origins").Get<string[]>();
//Console.WriteLine(strs); 
Console.WriteLine(string.Join("|", strs.Where(s => s != null)));
var jwt = config.GetSection("Api:Jwt").Get<JWT>();
Console.WriteLine(config["Api:Jwt:Secret"]);
Console.WriteLine(jwt.Secret);
Console.WriteLine(jwt.Audience);
Console.WriteLine(string.Join(",", jwt.Ids));
Console.WriteLine(config["Age"]);
Console.WriteLine("Hello, End");
