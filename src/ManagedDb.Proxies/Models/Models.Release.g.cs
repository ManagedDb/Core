
using ManagedDb.Dtos.Models;

namespace ManagedDb.Proxies.Models
{
#if !DEBUG

    public class product : MdbBaseEntity
    {
        public string title { get; set; }
        public string category { get; set; }
        public int price { get; set; }
    }
    
    public class todo : MdbBaseEntity
    {
        public string title { get; set; }
        public int rank { get; set; }
        public bool isDone { get; set; }
    }
    
    public class user : MdbBaseEntity
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dateBirth { get; set; }
    }
    
#endif
}