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
    }
}
