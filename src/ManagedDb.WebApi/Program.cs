using ManagedDb.WebApi.Models;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Reflection.Emit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ManagedDbApiOptions>(
    builder.Configuration.GetSection(ManagedDbApiOptions.ConfigKey));

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// register data types
//var options = app.Services.GetRequiredService<IOptions<ManagedDbApiOptions>>();
//var pathes = options.Value.EntityListPath
//    .Split(",")
//    .ToArray();
//var mdbAssembly = DataTypeCreator.RegisterDataTypes(pathes);

app.Run();
