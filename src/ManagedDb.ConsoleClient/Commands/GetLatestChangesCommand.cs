﻿using Cocona;
using ManagedDb.Core.Features.GetLatestChanges;
using ManagedDb.Core.Features.PullRequests;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ManagedDb.ConsoleClient.Commands;

public class GetLatestChangesCommand
{
    private readonly IPullRequestService prService;
    private readonly ILogger<GetLatestChangesCommand> logger;

    public GetLatestChangesCommand(
        IPullRequestService prService,
        ILogger<GetLatestChangesCommand> logger)
    {
        this.prService = prService;
        this.logger = logger;
    }

    [Command("github")]
    public async Task Handle(string pathToSave, string repoPath)
    {
        this.logger.LogInformation("Path to save: {pathToSave}", pathToSave);
        this.logger.LogInformation("Repo path: {repoPath}", repoPath);

        var changes = await this.prService
            .GetChangesAsync(GetChangesModeEnum.MainBranch);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var jsonContent = JsonSerializer.Serialize(
            changes,
            jsonSerializerOptions);

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