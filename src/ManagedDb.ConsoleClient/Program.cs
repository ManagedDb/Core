// See https://aka.ms/new-console-template for more information

using Cocona;
using ManagedDb.ConsoleClient.Commands;
using ManagedDb.Core;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("1.1");

var builder = CoconaApp.CreateBuilder();

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

app.AddCommands<MyDummyCommand>();

//app.AddCommands<GetLatestChangesCommand>();

Console.WriteLine("1.7");

app.Run();