using ManagedDb.Core.Features.PullRequests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ManagedDb.Core.Features.GetLatestChanges;

public class GitHubPullRequestService : IPullRequestService
{
    private readonly HttpClient ghClient;
    private readonly IOptions<ManagedDbOptions> options;

    public GitHubPullRequestService(
        IHttpClientFactory httpClientFactory,
        IOptions<ManagedDbOptions> options)
    {
        Console.WriteLine("0.1");
        this.ghClient = httpClientFactory.CreateClient("github");
        this.options = options;
    }

    public async Task<EntityChange[]> GetChangesAsync(GetChangesModeEnum mode)
    {
        var data = await this.CallApiAsync();

        var result = new List<EntityChange>();  

        foreach(var changedFile in data) 
        {
            var changes = this.ParseChanges(changedFile);
            result.AddRange(changes);
        }

        return result.ToArray();
    }

    private EntityChange[] ParseChanges(GitHubPullRequestResponseModel changedFile)
    {
        var name = this.GetFileName(changedFile.FileName);
        var path = changedFile.FileName;

        var headers = this.GetHeaders(changedFile);

        var diffOutput = changedFile.Patch;

        var lines = diffOutput.Split('\n');

        var addedRows = new List<(int RowNumber, string Row)>();
        var updatedRows = new List<(int RowNumber, string Original, string Updated)>();
        var removedRows = new List<(int RowNumber, string Row)>();

        int rowNumber = 0;

        foreach (var line in lines)
        {
            if (!line.StartsWith("+") && !line.StartsWith("-"))
            {
                rowNumber++;
            }

            if (line.StartsWith("+"))
            {
                addedRows.Add((rowNumber, line.Substring(1)));
            }
            else if (line.StartsWith("-"))
            {
                removedRows.Add((rowNumber, line.Substring(1)));
            }
        }

        // Check for updates
        foreach (var addedRow in addedRows.ToList())
        {
            foreach (var removedRow in removedRows.ToList())
            {
                var addedId = this.GetValues(addedRow.Row)[0];
                var removedId = this.GetValues(removedRow.Row)[0];

                if (addedId == removedId)
                {
                    updatedRows.Add((addedRow.RowNumber, removedRow.Row, addedRow.Row));
                    addedRows.Remove(addedRow);
                    removedRows.Remove(removedRow);
                    break;
                }
            }
        }

        var result = new List<EntityChange>();

        foreach (var addedRow in addedRows) 
        {
            var rowVals = this.GetValues(addedRow.Row);

            var originalFields = new Dictionary<string, string>();

            for (int i = 0; i < headers.Length; i++) 
            {
                originalFields.Add(headers[i], rowVals[i]);
            }

            result.Add(new EntityChange(
                name,
                path,
                addedRow.RowNumber,
                EntityChangeTypeEnum.Added,
                originalFields));
        }

        foreach (var removedRow in removedRows) 
        {
            var rowVals = this.GetValues(removedRow.Row);

            var originalFields = new Dictionary<string, string>();

            for (int i = 0; i < headers.Length; i++)
            {
                originalFields.Add(headers[i], rowVals[i]);
            }

            result.Add(new EntityChange(
                name,
                path,
                removedRow.RowNumber,
                EntityChangeTypeEnum.Removed,
                originalFields));
        }

        foreach(var updatedRow in updatedRows) 
        {
            var originRowVals = this.GetValues(updatedRow.Original);
            var updatedRowVals = this.GetValues(updatedRow.Updated);

            var originalFields = new Dictionary<string, string>();
            var updatedFields = new Dictionary<string, string>();

            for (int i = 0; i < headers.Length; i++)
            {
                originalFields.Add(headers[i], originRowVals[i]);

                if (originRowVals[i] != updatedRowVals[i]) 
                {
                    updatedFields.Add(headers[i], updatedRowVals[i]);
                }
            }

            if(!updatedFields.Any()) 
            {
                continue;
            }

            result.Add(new EntityChange(
                name,
                path,
                updatedRow.RowNumber,
                EntityChangeTypeEnum.Updated,
                originalFields,
                updatedFields));
        }

        return result.ToArray();
    }

    private string[] GetHeaders(GitHubPullRequestResponseModel changedFile) 
    {
        var headers = changedFile.Patch
            .Split('\n')[1]
            .Split(",")
            .Select(x => x.Trim())
            .ToArray();
        return headers;
    }

    public string[] GetValues(string row) 
    {
        return row
            .Split(",")
            .Select(x => x.Trim())
            .ToArray();
    }

    private string GetFileName(string path) => path.Split("/").Last().Split('.').First();

    private async Task<GitHubPullRequestResponseModel[]> CallApiAsync() 
    {
        var url = $"repos/{this.options.Value.Project}/{this.options.Value.Repository}/pulls/{this.options.Value.PrId}/files";
        var response = await this.ghClient
            .GetFromJsonAsync<GitHubPullRequestResponseModel[]>(url);
        return response;
    }
}

public class GitHubPullRequestResponseModel 
{
    [JsonPropertyName("sha")]
    public string Sha { get; set; }

    [JsonPropertyName("filename")]
    public string FileName { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("additions")]
    public int Additions { get; set; }

    [JsonPropertyName("deletions")]
    public int Deletions { get; set; }

    [JsonPropertyName("changes")]
    public int Changes { get; set; }

    [JsonPropertyName("blob_url")]
    public string BlobUrl { get; set; }

    [JsonPropertyName("raw_url")]
    public string RawUrl { get; set; }

    [JsonPropertyName("contents_url")]
    public string ContentsUrl { get; set; }

    [JsonPropertyName("patch")]
    public string Patch { get; set; }
}
