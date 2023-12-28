// See https://aka.ms/new-console-template for more information

using Cocona;
using ManagedDb.ConsoleClient.Commands;
using ManagedDb.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("1.1");

var builder = CoconaApp.CreateBuilder(args);

string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
string strWorkPath = Path.GetDirectoryName(strExeFilePath);

var appsettingsPath = Path.Combine(strWorkPath, "appsettings.json");
builder.Configuration
    .AddJsonFile(appsettingsPath)
    .AddEnvironmentVariables();

Console.WriteLine("1.2");

builder.Services.AddLogging();

Console.WriteLine("1.3");
builder.Services.Configure<ManagedDbOptions>(
    builder.Configuration.GetSection(ManagedDbOptions.ConfigKey));

Console.WriteLine("1.4");

builder.Services.AddManagedDb();

Console.WriteLine("1.5");

var app = builder.Build();

Console.WriteLine("1.6");

//app.AddCommands<MyDummyCommand>();

app.AddCommands<GetLatestChangesCommand>();

Console.WriteLine("1.7");

app.Run();