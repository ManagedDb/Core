using ManagedDb.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ManagedDb.WebApi.Controllers;

[Route("odata/[controller]")]
public class ODataBaseController<TEntity> : ODataController
    where TEntity : BaseEntity
{
}
