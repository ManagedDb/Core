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

        public bool Validate(
            string pathToCsv, 
            string pathToEntitySchema) 
        {
            this.LogEntityFile(pathToCsv);
            this.LogEntityFile(pathToEntitySchema);

            return this.InternalValidate(
                pathToCsv, 
                pathToEntitySchema);
        }

        private bool InternalValidate(string pathToCsv, string pathToEntitySchema) 
        {
            throw new NotImplementedException();
        }

        private void LogEntityFile(string pathToCsv) => 
            this.logger.LogDebug("{csvFile} is exist: {isExist}",
                pathToCsv,
                File.Exists(pathToCsv));

        private void LogEntitySchema(string pathToEntitySchema) =>
            this.logger.LogDebug("{entityFile} is exist: {isExist}",
                pathToEntitySchema,
                File.Exists(pathToEntitySchema));
    }
}
