using Cocona;
using ManagedDb.Core;
using ManagedDb.Core.Features.GetLatestChanges;
using ManagedDb.Core.Features.PullRequests;
using ManagedDb.Core.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ManagedDb.ConsoleClient.Commands;

public class GitHubPrChangesV1Command
{
    private readonly IPullRequestService prService;
    private readonly IOptions<ManagedDbOptions> options;
    private readonly ILogger<GitHubPrChangesV1Command> logger;

    public GitHubPrChangesV1Command(
        IPullRequestService prService,
        IOptions<ManagedDbOptions> options,
        ILogger<GitHubPrChangesV1Command> logger)
    {
        this.prService = prService;
        this.options = options;
        this.logger = logger;
    }

    [Command("githubprchangesv1")]
    public async Task Handle()
    {
        var pathToSave = options.Value.PathToSave;

        this.logger.LogInformation("Path to save: {pathToSave}", pathToSave);
        
        var changes = await this.prService
            .GetChangesAsync(GetChangesModeEnum.MainBranch);

        var jsonContent = JsonSerializer.Serialize(
            changes,
            MdbHelper.GetJsonSerializerOptions);

        if (string.IsNullOrWhiteSpace(pathToSave))
        {
            pathToSave = Path.Combine(
                Environment.CurrentDirectory,
                "changes.json");
        }

        var dirNames = Path.GetDirectoryName(pathToSave);

        if (!Directory.Exists(dirNames) && !string.IsNullOrWhiteSpace(dirNames))
        {
            Directory.CreateDirectory(dirNames);
        }

        File.WriteAllText(
            pathToSave,
            jsonContent);
    }
}