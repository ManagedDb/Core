using ManagedDb.Core.Features.PullRequests;
using System.Text.Json;

namespace ManagedDb.Core.Units;

[TestClass]
public class LocalPullRequestServiceTests
{
    [TestMethod]
    public async Task GetChanges_FromMainBranch_Should_Work()
    {
        // Arrange
        var service = new LocalPullRequestService();

        // Act
        var changes = await service.GetChangesAsync(GetChangesModeEnum.MainBranch);

        // Assert
        Assert.IsNotNull(changes);

    }

    [TestMethod]
    public async Task GetChanges_LastCommit_Should_Work()
    {
        // Arrange
        var service = new LocalPullRequestService();

        // Act
        var changes = await service.GetChangesAsync(GetChangesModeEnum.LastCommit);

        // Assert
        Assert.IsNotNull(changes);
    }
}