using ManagedDb.Core.Features.DataProxyCreators;
using ManagedDb.Dtos.Models;
using ManagedDb.Proxies;
using ManagedDb.WebApi.Models;
using Microsoft.AspNetCore.OData;

var mdbTypes = MdbDataProxyHelper.GetMdbDataTypes();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ManagedDbApiOptions>(
    builder.Configuration.GetSection(ManagedDbApiOptions.ConfigKey));

builder.Services.AddSingleton<BaseMdbDbContext, MdbDbContext>(_ => 
{
    var db = new MdbDbContext(mdbTypes);

    db.Database.EnsureCreated();

    return db;
});

var emdModel = ControllerHelper.RegisterODataEntities(mdbTypes);

//builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents(
            "mdb/odata",
            emdModel));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// build app.
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// run app.
app.Run();
