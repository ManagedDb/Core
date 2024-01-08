using ManagedDb.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ManagedDb.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DataTypesController : ControllerBase
{
    private readonly ILogger<DataTypesController> logger;
    private readonly IOptions<ManagedDbApiOptions> options;

    public DataTypesController(
        ILogger<DataTypesController> logger,
        IOptions<ManagedDbApiOptions> options)
    {
        this.logger = logger;
        this.options = options;
    }

    //[HttpPost("register-types")]
    //public IActionResult Post()
    //{
    //    var pathes = this.options.Value.EntityListPath
    //        .Split(",")
    //        .ToArray();

    //    DataTypeCreator.RegisterDataTypes(pathes);

    //    return Ok();
    //}

    [HttpGet("types")]
    public IActionResult Get() 
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName();

        Console.WriteLine(assemblyName.FullName);
        Console.WriteLine(assemblyName.Name);

        var types = assembly.GetTypes()
            .Where(t => t.Namespace == "ManagedDb.EntityDataTypes.Proxies.Models")
            .Select(t => t.Name)
            .ToList();
        
        return Ok(types);
    }
}