using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDb.Core.Features.SchemaValidators
{
    /// <summary>
    /// Entity schema class.
    /// Represent the schema of entity. 
    /// </summary>
    public class EntitySchema
    {
        /// <summary>
        /// Will override the entity name. If not provided, a folder name will be used as an entity name.
        /// </summary>
        public string? EntityName { get; set; }

        /// <summary>
        /// List of fields with metadata.
        /// </summary>
        public Dictionary<string, EntityField>? Fields { get; set; }
    }

    /// <summary>
    /// Metadata for entity field.
    /// </summary>
    public class EntityField 
    {
        /// <summary>
        /// Field type. Represent type of a field. Can not be empty or null. 
        /// </summary>
        /// <example>
        /// int, float, string, datetime and etc.
        /// </example>
        public string FieldType { get; set; } = string.Empty;

        /// <summary>
        /// Represent if field can be empty or null.
        /// </summary>
        public bool? IsRequired { get; set; }

        /// <summary>
        /// Override a field name. if not provided, will be taken from the first row of data file.
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Field order
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Field string format
        /// </summary>
        /// <example>
        /// if a field data is datetime, can be used as "yyyy.MM.dd"
        /// </example>
        public string? Format { get; set; }

        /// <summary>
        /// Represent default value. 
        /// </summary>
        public string? DefaultValue { get; set; }

        /// <summary>
        /// Is a field Primary Key. Entity can has only one PK.
        /// </summary>
        public bool? IsPk { get; set; }

        /// <summary>
        /// Is a field Foreing key. 
        /// If provided you should also cover <see cref="FkEntityName"/> and <see cref="FkEntityField"/>
        /// </summary>
        public bool? IsFk { get; set; }

        /// <summary>
        /// Requires only if <see cref="IsFk"/> provided.
        /// Represent Entity for Fk.
        /// </summary>
        public string? FkEntityName { get; set; }

        /// <summary>
        /// Requires only if <see cref="IsFk"/> provided.
        /// Represent Entity field for Fk. Should be PK.
        /// </summary>
        public string? FkEntityField { get; set; }
    }
}
