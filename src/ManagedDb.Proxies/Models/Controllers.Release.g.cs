
using ManagedDb.Dtos.Models;

namespace ManagedDb.Proxies.Models
{

#if !DEBUG    
    public class productController : MdbOdataControllerBase<product>
    {
        public productController(BaseMdbDbContext dbContext)
            : base(dbContext)
        {
        }
}

    public class todoController : MdbOdataControllerBase<todo>
    {
        public todoController(BaseMdbDbContext dbContext)
            : base(dbContext)
        {
        }
}
    
    public class userController : MdbOdataControllerBase<user>
    {
        public userController(BaseMdbDbContext dbContext)
            : base(dbContext)
        {
        }
}
    
#endif
}