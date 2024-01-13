using ManagedDb.Dtos.Models;
using System.Runtime.CompilerServices;

namespace ManagedDb.Proxies;

public static class MdbDataProxyHelper
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Type[] GetMdbDataTypes()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var types = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(MdbBaseEntity)))
            .ToArray();

        return types;
    }
}
