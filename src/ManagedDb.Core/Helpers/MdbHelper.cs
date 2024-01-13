using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManagedDb.Core.Helpers
{
    public static class MdbHelper
    {
        public static JsonSerializerOptions GetJsonSerializerOptions => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public static string ManagedDbAppDomainName => "ManagedDb.Data.Proxies";
        public static string ManagedDbAssemblyName => "ManagedDb.Data.Proxies";
        public static string ManagedDModuleName => "ManagedDb.Data.Proxies";
    }
}
