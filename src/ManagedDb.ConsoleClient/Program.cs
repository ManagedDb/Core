// See https://aka.ms/new-console-template for more information

using Cocona;
using ManagedDb.ConsoleClient.Commands;
using ManagedDb.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder(args);

string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
string strWorkPath = Path.GetDirectoryName(strExeFilePath) ?? string.Empty;

var appsettingsPath = Path.Combine(strWorkPath, "appsettings.json");
builder.Configuration
    .AddJsonFile(appsettingsPath)
    .AddEnvironmentVariables();

builder.Services.AddLogging();

builder.Services.Configure<ManagedDbOptions>(
    builder.Configuration.GetSection(ManagedDbOptions.ConfigKey));

builder.Services.AddManagedDb();

var app = builder.Build();

// add pr changes command
app.AddCommands<GitHubPrChangesV1Command>();
app.AddCommands<GitHubPrChangesV2Command>();

// add create db command
app.AddCommands<CreateDbCommand>();

app.Run();