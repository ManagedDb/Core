using ManagedDb.Dtos.Models;

namespace ManagedDb.Proxies.Models;

#if DEBUG

public class product : MdbBaseEntity 
{
    public string title { get; set; }

    public string category { get; set; }

    public int price { get; set; }
}

#endif