using ManagedDb.Core.Features.DataProxyCreators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDb.Data.Proxies
{
#if DEBUG
    public class Product : MdbBaseEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public int Price { get; set; }
    }

    public class Todo : MdbBaseEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Rank { get; set; }
        public bool IsDone { get; set; }
    }

    public class User : MdbBaseEntity
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DateBirth { get; set; }
    }
#endif
}
