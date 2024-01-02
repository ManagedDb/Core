using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDb.Core.Features.SchemaValidators
{
    public class EntitySchema
    {
        public string Name { get; set; }

        public bool? IsRequired { get; set; }

        public EntityField[] Fields { get; set; } = Array.Empty<EntityField>();
    }

    public class EntityField 
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public int? Order { get; set; }

        public string? Format { get; set; }
    }
}
