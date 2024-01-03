using Cocona;
using ManagedDb.Core;
using ManagedDb.Core.Features.GetLatestChanges;
using ManagedDb.Core.Features.PullRequests;
using ManagedDb.Core.Features.SchemaConvertors;
using ManagedDb.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ManagedDb.ConsoleClient.Commands;

public class GitHubPrChangesV2Command
{
    private readonly IPullRequestService prService;
    private readonly SchemaConvertor schemaConvertor;
    private readonly IOptions<ManagedDbOptions> options;
    private readonly ILogger<GitHubPrChangesV2Command> logger;

    public GitHubPrChangesV2Command(
        [FromKeyedServices("local")]IPullRequestService prService,
        SchemaConvertor schemaConvertor,
        IOptions<ManagedDbOptions> options,
        ILogger<GitHubPrChangesV2Command> logger)
    {
        this.prService = prService;
        this.schemaConvertor = schemaConvertor;
        this.options = options;
        this.logger = logger;
    }

    [Command("githubprchangesv2")]
    public async Task Handle()
    {
        var pathToSave = options.Value.PathToSave;

        this.logger.LogInformation("Path to save: {pathToSave}", pathToSave);
        
        var changes = await this.prService
            .GetChangesAsync(GetChangesModeEnum.MainBranch);

        var converted = this.schemaConvertor.ConvertBySchemaAsync(changes);

        var jsonContent = JsonSerializer.Serialize(
            converted,
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