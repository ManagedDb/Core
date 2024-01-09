//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.AspNetCore.OData.Routing.Controllers;

//namespace ManagedDb.Core.Features.DataProxyCreators;

//[Route("odata/[controller]")]
//public abstract class ODataBaseController<TEntity> : ODataController
//    where TEntity : MdbBaseEntity
//{
//    private readonly ApplicationDbContext dbContext;

//    public ODataBaseController(ApplicationDbContext dbContext)
//    {
//        this.dbContext = dbContext;
//    }

//    [HttpGet]
//    [EnableQuery]
//    protected IQueryable<TEntity> GetQueryable()
//    {
//        return this.dbContext.Set<TEntity>();
//    }
//}
