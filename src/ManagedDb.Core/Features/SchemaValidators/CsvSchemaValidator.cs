using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDb.Core.Features.SchemaValidators
{
    public class CsvSchemaValidator
    {
        private readonly ILogger<CsvSchemaValidator> logger;

        public CsvSchemaValidator(
            ILogger<CsvSchemaValidator> logger)
        {
            this.logger = logger;
        }

        public async Task<bool> ValidateAsync(
            string pathToCsv, 
            string pathToEntitySchema) 
        {
            var schema = await this.GetSchemaAsync(pathToEntitySchema);

            if (schema == null) 
            {
                return true;
            }

            this.LogEntityFile(pathToEntitySchema);

            throw new InvalidOperationException();
        }

        private void LogEntityFile(string pathToCsv) => 
            this.logger.LogDebug("{csvFile} is exist: {isExist}",
                pathToCsv,
                File.Exists(pathToCsv));

        private void LogEntitySchema(string pathToEntitySchema) =>
            this.logger.LogDebug("{entityFile} is exist: {isExist}",
                pathToEntitySchema,
                File.Exists(pathToEntitySchema));

        private async Task<EntitySchema?> GetSchemaAsync(string pathToEntitySchema) 
        {
            this.LogEntitySchema(pathToEntitySchema);

            if (!File.Exists(pathToEntitySchema))
            {
                return null;
            }

            var schema = await File.ReadAllTextAsync(pathToEntitySchema);
            return System.Text.Json.JsonSerializer.Deserialize<EntitySchema>(schema);
        }
    }
}
