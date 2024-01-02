using LibGit2Sharp;
using ManagedDb.Core.Features.PullRequests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDb.Core.Features.SchemaConvertors
{
    public class SchemaConvertor
    {
        private readonly SchemaProvider schemaProvider;
        private readonly ILogger<SchemaConvertor> logger;
        
        public SchemaConvertor(
            SchemaProvider schemaProvider,
            ILogger<SchemaConvertor> logger)
        {
            this.schemaProvider = schemaProvider;
            this.logger = logger;
        }

        public async Task<EntityChange[]> ValidateAsync(EntityChange[] changes) 
        {
            var result = new List<EntityChange>();

            foreach (var item in changes) 
            {
                var schema = await this.schemaProvider.GetSchemaAsync(item.Path);

                if (schema == null)
                {
                    this.logger.LogWarning($"Schema not found for {item.Path}");
                    continue;
                }

                var newEntity = this.ConvertEntityBasedOnSchema(item, schema);

                this.ValidateEntity(item, schema);

                result.Add(newEntity);
            }

            return result.ToArray();
        }

        private EntityChange ConvertEntityBasedOnSchema(
            EntityChange entity, 
            EntitySchema schema)
        {
            entity = entity with
            {
                Name = schema.EntityName,
            };
            return entity;
        }

        private void ValidateEntity(
            EntityChange entity, 
            EntitySchema schema) 
        {
            if (schema.Fields == null)
                return;

            if (entity.ChangeType == EntityChangeTypeEnum.Added)
            {
                this.ValidateFields(entity, schema, entity.OriginalFields);
            }
            else if (entity.ChangeType == EntityChangeTypeEnum.Updated)
            {
                this.ValidateFields(entity, schema, entity.UpdatedFields);
            }
            else if (entity.ChangeType == EntityChangeTypeEnum.Removed)
            {
                // TODO: check FKs
            }
            else 
            {
                this.logger.LogInformation(
                    "ChangeType: {changeType} is not correct for entity: {entity}",
                    entity.ChangeType,
                    entity);
            }
        }

        private void ValidateFields(
            EntityChange entity,
            EntitySchema schema, 
            Dictionary<string, string>? fields) 
        {
            if (fields == null)
                return;

            var isIncorrect = false;

            foreach (var f in fields)
            {
                var fName = f.Key;
                var fValue = f.Value;

                schema.Fields.TryGetValue(
                    fName,
                    out var schemaFieldResult);

                if (schemaFieldResult == null)
                {
                    this.logger.LogInformation(
                        "Field {fieldName} not found in schema for entity {entity}",
                    fName,
                        entity);
                    continue;
                }

                var typeIsCorrect = this.IsValueCorrect(fValue, schemaFieldResult);

                this.logger.LogError(
                    "field name: {fieldName} value: {fieldValue} is not correct. entity: {entity}. schema: {schema}",
                    fName,
                fValue,
                    entity,
                    schema);
            }

            if (isIncorrect)
                throw new Exception("Incorrect entity");
        }

        private bool IsValueCorrect(string value, EntityField entityField) 
        {
            if (string.IsNullOrWhiteSpace(value) && !(entityField.IsRequired == true))
                return true;

            if (entityField.FieldType == "bool")
            {
                return bool.TryParse(value, out var _);
            }
            else if (entityField.FieldType == "int")
            {
                return int.TryParse(value, out var _);
            }
            else if (entityField.FieldType == "float")
            {
                return float.TryParse(value, out var _);
            }
            else if (entityField.FieldType == "string")
            {
                return !string.IsNullOrWhiteSpace(value);
            }
            else if (entityField.FieldType == "guid")
            {
                return Guid.TryParse(value, out var _);
            }
            else if(entityField.FieldType == "datetime")
            {
                return DateTime.TryParse(value, out var _);
            }
            else 
            {
                this.logger.LogInformation(
                    "{entityType} is unknown",
                    entityField.FieldType);
                return true;
            }
        }
    }
}
