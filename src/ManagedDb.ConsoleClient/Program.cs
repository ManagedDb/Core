// See https://aka.ms/new-console-template for more information

using ManagedDb.Core.Features.GetLatestChanges;
using System.Text.Json;

var command = args.Length > 0 && args[0] == "commit" 
    ? GetChangesModeEnum.LastCommit
    : GetChangesModeEnum.MainBranch;

var service = new GetLatestChangesService();

var changes = service.GetChanges(command);

var jsonSerializerOptions = new JsonSerializerOptions
{
    WriteIndented = true
};

var jsonContent = JsonSerializer.Serialize(
    changes,
    jsonSerializerOptions);

var fileName = @"changes.json";

if (File.Exists(fileName)) 
{
    File.Delete(fileName);
}

File.WriteAllText(
    fileName,
    jsonContent);
