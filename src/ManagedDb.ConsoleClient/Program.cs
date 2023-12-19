// See https://aka.ms/new-console-template for more information

using ManagedDb.Core.Features.GetLatestChanges;
using System.Text.Json;

var command = args.Length > 0 && args[0] == "commit" 
    ? GetChangesModeEnum.LastCommit
    : GetChangesModeEnum.MainBranch;

var pathToSave = args.Length > 1
    ? args[1]
    : string.Empty;

var repoPath = args.Length > 2
    ? args[2]
    : Environment.GetEnvironmentVariable("MDBRootFolder");

Console.WriteLine($"Changes event: {command}");
Console.WriteLine($"Path to save: {pathToSave}");
Console.WriteLine($"Repo path: {repoPath}");

if (string.IsNullOrWhiteSpace(repoPath))
{
    Console.WriteLine("Repo path is empty");
    return;
}

var service = new GetLatestChangesService(repoPath);

var changes = service.GetChanges(command);

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

if(!Directory.Exists(dirNames) && !string.IsNullOrWhiteSpace(dirNames))
{
    Directory.CreateDirectory(dirNames);
}

File.WriteAllText(
    pathToSave,
    jsonContent);
