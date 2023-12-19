// See https://aka.ms/new-console-template for more information

using ManagedDb.Core.Features.GetLatestChanges;
using System.Text.Json;

var command = args.Length > 0 && args[0] == "commit" 
    ? GetChangesModeEnum.LastCommit
    : GetChangesModeEnum.MainBranch;

var pathToSave = args.Length > 1
    ? args[1]
    : string.Empty;

Console.WriteLine($"Changes event: {command}");
Console.WriteLine($"Path to save: {pathToSave}");

var service = new GetLatestChangesService();

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

if(!Directory.Exists(dirNames))
{
    Directory.CreateDirectory(pathToSave);
}

if (File.Exists(pathToSave)) 
{
    File.Delete(pathToSave);
}

File.WriteAllText(
    pathToSave,
    jsonContent);
