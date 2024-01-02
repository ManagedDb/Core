using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDb.Core.Features.SchemaConvertors
{
    /// <summary>
    /// Entity schema class.
    /// Represent the schema of entity.
    /// </summary>
    /// <param name="EntityName">Will override the entity name. If not provided, a folder name will be used as an entity name.</param>
    /// <param name="Fields">List of fields with metadata.</param>
    public record EntitySchema(
        string? EntityName, 
        Dictionary<string, EntityField>? Fields);

    /// <summary>
    /// Metadata for entity field.
    /// </summary>
    /// <param name="FieldType">Field type. Represent type of a field. Can not be empty or null (bool, int, float, string, datetime, guid and etc.).</param>
    /// <param name="IsRequired">Represent if field can be empty or null.</param>
    /// <param name="Label">Override a field name. if not provided, will be taken from the first row of data file.</param>
    /// <param name="Order">Field order</param>
    /// <param name="Format">Field string format. if a field data is datetime, can be used as "yyyy.MM.dd"</param>
    /// <param name="DefaultValue">Represent default value. </param>
    /// <param name="IsPk">Is a field Primary Key. Entity can has only one PK.</param>
    /// <param name="IsFk">Is a field Foreing key. If provided you should also cover <see cref="FkEntityName"/> and <see cref="FkEntityField"/>.</param>
    /// <param name="FkEntityName">Requires only if <see cref="IsFk"/> provided. Represent Entity for Fk.</param>
    /// <param name="FkEntityField">Requires only if <see cref="IsFk"/> provided. Represent Entity field for Fk. Should be PK.</param>
    public record EntityField(
        string FieldType,
        bool? IsRequired,
        string? Label,
        int? Order,
        string? Format,
        string? DefaultValue,
        bool? IsPk,
        bool? IsFk,
        string? FkEntityName,
        string? FkEntityField);
}
