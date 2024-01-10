using Cocona;
using ManagedDb.Core.Features.DataProxyCreators;
using System.Runtime.CompilerServices;

namespace ManagedDb.ConsoleClient.Commands;

public class DataProxyGenerateCommand
{
    private readonly DataProxyClassesCreator dataProxyCreator;

    private const string PathToSave = $@"D:\Repositories\ManagedDb\Core\src\ManagedDb.Data.Proxies";
    private List<string> EntitySchemas = new List<string>() 
    {
        @"D:\Repositories\ManagedDb\Core\data\products\mdb.entity.schema.json",
        @"D:\Repositories\ManagedDb\Core\data\todos\mdb.entity.schema.json",
        @"D:\Repositories\ManagedDb\Core\data\users\mdb.entity.schema.json"
    };

    public DataProxyGenerateCommand(DataProxyClassesCreator dataProxyCreator)
    {
        this.dataProxyCreator = dataProxyCreator;
    }

    [Command("dataproxygenerate")]
    public async Task Handle()
    {
        var pathes = string.Join(",", EntitySchemas);

        this.dataProxyCreator.GenerateProject(
            pathes,
            PathToSave);

        Console.WriteLine("Hello World!");
    }
}
