// See https://aka.ms/new-console-template for more information

using Cocona;
using ManagedDb.ConsoleClient.Commands;
using ManagedDb.Core;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddLogging();

builder.Services.Configure<ManagedDbOptions>(
    builder.Configuration.GetSection(ManagedDbOptions.ConfigKey));

builder.Services.AddManagedDb();

var app = builder.Build();

app.AddCommands<GetLatestChangesCommand>();

app.Run();