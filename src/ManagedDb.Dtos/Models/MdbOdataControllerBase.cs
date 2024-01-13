using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ManagedDb.Dtos.Models;

[Route("mdb/odata/[controller]")]
public abstract class MdbOdataControllerBase<TEntity> : ODataController
    where TEntity : MdbBaseEntity, new()
{
    private readonly BaseMdbDbContext dbContext;

    public MdbOdataControllerBase(BaseMdbDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable<TEntity> GetQueryable()
    {
        return this.dbContext.Set<TEntity>();
    }
}
