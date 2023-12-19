using ManagedDb.Core.Features.GetLatestChanges;
using System.Text.Json;

namespace ManagedDb.Core.Units;

[TestClass]
public class GetLatestChangesServiceTests
{
    [TestMethod]
    public void GetChanges_FromMainBranch_Should_Work()
    {
        // Arrange
        var service = new GetLatestChangesService();

        // Act
        var changes = service.GetChanges(GetChangesModeEnum.MainBranch);

        // Assert
        Assert.IsNotNull(changes);

    }

    [TestMethod]
    public void GetChanges_LastCommit_Should_Work()
    {
        // Arrange
        var service = new GetLatestChangesService();

        // Act
        var changes = service.GetChanges(GetChangesModeEnum.LastCommit);

        // Assert
        Assert.IsNotNull(changes);
    }
}