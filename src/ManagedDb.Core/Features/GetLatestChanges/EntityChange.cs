namespace ManagedDb.Core.Features.PullRequests;

public record EntityChange(
    string Name,
    string Path,
    int RowNumber,
    EntityChangeTypeEnum ChangeType,
    Dictionary<string, string>? OriginalFields = null,
    Dictionary<string, string>? UpdatedFields = null);

public enum EntityChangeTypeEnum
{
    Added = 1,
    Updated,
    Removed
}

public enum GetChangesModeEnum
{
    LastCommit,
    MainBranch
}