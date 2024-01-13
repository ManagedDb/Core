using ManagedDb.Dtos.Models;

namespace ManagedDb.Proxies.Models;

#if DEBUG

public class productController : MdbOdataControllerBase<product>
{
    public productController(BaseMdbDbContext dbContext)
        : base(dbContext)
    {
    }
}

#endif