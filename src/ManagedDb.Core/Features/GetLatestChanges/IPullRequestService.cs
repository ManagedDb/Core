using ManagedDb.Core.Features.PullRequests;

namespace ManagedDb.Core.Features.GetLatestChanges;

public interface IPullRequestService
{
    Task<EntityChange[]> GetChangesAsync(GetChangesModeEnum mode);
}