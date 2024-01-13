using ManagedDb.Core.Features.DataProxyCreators;
using ManagedDb.Core.Helpers;
using ManagedDb.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ManagedDb.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DataTypesController : ControllerBase
{
    private readonly MdbDbContext dbContext;
    private readonly ILogger<DataTypesController> logger;

    public DataTypesController(
        MdbDbContext dbContext,
        ILogger<DataTypesController> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    [HttpGet("types")]
    public IActionResult Get() 
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName == MdbHelper.ManagedDbAssemblyName)
            .FirstOrDefault();

        if(assembly == null)
            return NotFound();

        var types = assembly.GetTypes()
            .Where(t => t.Namespace == "ManagedDb.EntityDataTypes.Proxies.Models")
            .Select(t => t.Name)
            .ToList();
        
        return Ok(types);
    }
}