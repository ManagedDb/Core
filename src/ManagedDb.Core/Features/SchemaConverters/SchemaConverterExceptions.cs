namespace ManagedDb.Core.Features.SchemaConverters;

public abstract class SchemaConverterExceptions : MdbBaseException
{
    public string EntityName { get; private set; }

    public SchemaConverterExceptions(string entityName, string message)
        : base(message)
    {
        EntityName = entityName;
    }
}

public class SchemaIsIncorrectException : SchemaConverterExceptions
{
    public SchemaIsIncorrectException(string entityName, string message)
        : base(entityName, message)
    {
    }
}

public class MissingRequiredFieldException : SchemaConverterExceptions
{
    public MissingRequiredFieldException(string entityName, string message)
        : base(entityName, message)
    {
    }
}

public class FieldTypeIsNotCorrect : SchemaConverterExceptions
{
    public FieldTypeIsNotCorrect(string entityName, string message)
        : base(entityName, message)
    {
    }
}
