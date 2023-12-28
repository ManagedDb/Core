using Cocona;
using ManagedDb.Core;
using ManagedDb.Core.Features.GetLatestChanges;
using ManagedDb.Core.Features.PullRequests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ManagedDb.ConsoleClient.Commands;

public class GetLatestChangesCommand
{
    private readonly IPullRequestService prService;
    private readonly IOptions<ManagedDbOptions> options;
    private readonly ILogger<GetLatestChangesCommand> logger;

    public GetLatestChangesCommand(
        IPullRequestService prService)
    {
        Console.WriteLine("2.0");
        this.prService = prService;
    }

    public async Task Handle()
    {
        Console.WriteLine("2.1");

        return;

        var pathToSave = options?.Value?.PathToSave;
        var repoPath = options?.Value?.RepoPath;

        //this.logger.LogDebug("Path to save: {pathToSave}", pathToSave);
        //this.logger.LogDebug("Repo path: {repoPath}", repoPath);

        var changes = await this.prService
            .GetChangesAsync(GetChangesModeEnum.MainBranch);

        Console.WriteLine("2.2");

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var jsonContent = JsonSerializer.Serialize(
            changes,
            jsonSerializerOptions);

        Console.WriteLine("2.3");

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

        Console.WriteLine("2.4");

        File.WriteAllText(
            pathToSave,
            jsonContent);

        Console.WriteLine("2.5");
    }
}

public class MyDummyCommand 
{
    public Task Handle() 
    {
        Console.WriteLine("Hello world!!!");

        return Task.CompletedTask; 
    }
}