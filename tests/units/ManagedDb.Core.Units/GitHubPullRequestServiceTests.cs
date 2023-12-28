using ManagedDb.Core.Features.GetLatestChanges;
using ManagedDb.Core.Features.PullRequests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ManagedDb.Core.Units;

[TestClass]
public class GitHubPullRequestServiceTests
{
    private readonly IServiceProvider services;
    private readonly string token = "";

    public GitHubPullRequestServiceTests()
    {
        var options = Options.Create<ManagedDbOptions>(new ManagedDbOptions() 
        {
            GitHubBaseUrl = "https://api.github.com/",
            Project = "ManagedDb",
            Repository = "Core",
            PrId = 10,
            Token = token
        });

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddManagedDb(options);

        this.services = serviceCollection.BuildServiceProvider();
    }

    [TestMethod]
    public async Task GetChanges_Should_Work()
    {
        // Arrange
        if (string.IsNullOrWhiteSpace(this.token)) 
        {
            return;
        }

        var service = services.GetRequiredService<IPullRequestService>();

        // Act
        var changes = await service.GetChangesAsync(GetChangesModeEnum.MainBranch);

        Console.WriteLine(changes);

        // Assert
        Assert.IsNotNull(changes);
    }
}