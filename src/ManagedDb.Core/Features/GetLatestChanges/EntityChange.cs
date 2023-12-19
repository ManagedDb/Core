namespace ManagedDb.Core.Features.GetLatestChanges;

public record EntityChange(
    string Name,
    string Path,
    int rowNumber,
    EntityChangeTypeEnum ChangeType,
    Dictionary<string, string>? originalFields = null,
    Dictionary<string, string>? updatedFields = null);

public enum EntityChangeTypeEnum
{
    Added,
    Updated,
    Removed
}