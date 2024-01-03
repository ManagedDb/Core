using ManagedDb.Core.Features.SchemaConverters;
using ManagedDb.Core.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ManagedDb.Core.Features.SchemaConvertors
{
    public class SchemaProvider
    {
        private const string EntitySchemaFileName = "mdb.entity.schema.json";
        
        private readonly ILogger<SchemaProvider> logger;
        private readonly IOptions<ManagedDbOptions> options;

        private Dictionary<string, EntitySchema> schemas = new();

        public SchemaProvider(
            ILogger<SchemaProvider> logger,
            IOptions<ManagedDbOptions> options)
        {
            this.logger = logger;
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToCsv">../data/{entityName}/{entityName}.csv</param>
        /// <returns></returns>
        public virtual async Task<EntitySchema?> GetSchemaAsync(string pathToCsv)
        {
            var pathToEntitySchema = this.GetSchemaPath(pathToCsv);

            this.LogEntitySchema(pathToEntitySchema);

            if (!File.Exists(pathToEntitySchema))
            {
                return null;
            }

            var entity = this.GetSchema(pathToEntitySchema);

            if(entity == null)
            {
                var schema = await File.ReadAllTextAsync(pathToEntitySchema);
                entity = JsonSerializer.Deserialize<EntitySchema>(
                    schema, 
                    MdbHelper.GetJsonSerializerOptions);

                this.ValidateSchema(entity);

                this.AddSchema(pathToEntitySchema, entity);
            }

            return entity;
        }

        private void ValidateSchema(EntitySchema schema) 
        {
            if (schema.Fields == null || !schema.Fields.Any())
                return;

            var isIncorrect = false;

            var primaryKeyCount = schema.Fields.Count(x => x.Value.IsPk == true);
            if (primaryKeyCount != 1)
            {
                this.logger.LogError(
                    "Error for entity schema. PK amount should be 1 (now is {pkAmount}). Entity: {entity}",
                    primaryKeyCount,
                    schema);
                isIncorrect = true;
            }

            var fieldsWithoutTypes = schema.Fields.Where(x => string.IsNullOrEmpty(x.Value.FieldType));
            if (fieldsWithoutTypes.Any())
            {
                this.logger.LogError(
                    "Error for entity schema. Fields without types: {fields}. Entity: {entity}",
                    fieldsWithoutTypes,
                    schema);
                isIncorrect = true;
            }

            var fks = schema.Fields
                .Where(x => 
                    (x.Value.IsFk == true) 
                    && (string.IsNullOrEmpty(x.Value.FkEntityName) || string.IsNullOrEmpty(x.Value.FkEntityField)));
            if(fks.Any())
            {
                this.logger.LogError(
                    "Error for entity schema. FKs without entity name or field name: {fks}. Entity: {entity}", 
                    fks, 
                    schema);
                isIncorrect = true;
            }

            if (isIncorrect)
                throw new SchemaIsIncorrectException(
                    "Incorrect schema", 
                    schema.EntityName);
        }

        private void AddSchema(string pathToSchema, EntitySchema schema) =>
            this.schemas.TryAdd(pathToSchema, schema);

        private EntitySchema? GetSchema(string pathToSchema) =>
            this.schemas.TryGetValue(pathToSchema, out var schema) ? schema : null;

        private string GetSchemaPath(string pathToCsv) 
        {
            // pathToCsv = "//data//{entityName}//data.csv".
            // fullPath will be combined with curret repo path.
            // extract {entityName} from pathToCsv?
            var fullpath = Path.Combine(this.options.Value.RepoPath, pathToCsv);
            var dir = Directory.GetParent(fullpath).FullName;
            return Path.Combine(dir, EntitySchemaFileName);
        }

        private void LogEntitySchema(string pathToEntitySchema) =>
            this.logger.LogDebug("{entityFile} is exist: {isExist}",
                pathToEntitySchema,
                File.Exists(pathToEntitySchema));
    }
}
