using LibGit2Sharp;

namespace ManagedDb.Core.Features.GetLatestChanges;

public class GetLatestChangesService
{
    private readonly string dataFolderName = @"data";
    private readonly string fileExtension = ".csv";
    private readonly string repoPath;
    private readonly string mainBranchName = "main";

    public GetLatestChangesService(string repoPath)
    {
        this.repoPath = repoPath;
    }

    public GetLatestChangesService()
    {
        var currentDir = Environment.CurrentDirectory;
        var dir = new DirectoryInfo(currentDir);

        while(!Repository.IsValid(dir.FullName) && dir != null)
        {
            dir = dir.Parent;
        }

        if (dir == null)
        {
            ArgumentException.ThrowIfNullOrEmpty(currentDir);
        }

        this.repoPath = dir.FullName;
    }

    public ICollection<EntityChange> GetChanges(GetChangesModeEnum mode)
    {
        using var repo = new Repository(this.repoPath);

        var patches = mode == GetChangesModeEnum.LastCommit
            ? this.GetChangesBasedOnLastCommit(repo)
            : this.GetChangesBasedOnMainBranch(repo);

        var result = new List<EntityChange>();

        if (patches == null)
        {
            return result;
        }

        foreach (var entityPatch in patches)
        {
            var fieldNames = this.GetEntityFieldNames(entityPatch.Path);

            // path: data/entity/entity.csv
            // we need to take just entity
            var entityName = this.GetEntityName(entityPatch);

            // handle added entity
            var addedEntities = this.HandleLines(
                entityPatch.AddedLines,
                EntityChangeTypeEnum.Added,
                entityName,
                entityPatch.Path,
                fieldNames);

            result.AddRange(addedEntities);

            // handle removed entities
            var removedEntities = this.HandleLines(
                entityPatch.DeletedLines,
                EntityChangeTypeEnum.Removed,
                entityName,
                entityPatch.Path,
                fieldNames);

            result.AddRange(removedEntities);
        }

        return result;
    }

    private EntityChange[] HandleLines(
        List<Line> lines, 
        EntityChangeTypeEnum changeType,
        string entityName,
        string path,
        string[] fieldNames) 
    {
        return lines
            .Where(x => x.LineNumber > 1)
            .Select(x =>
                this.ConvertLineToEntityChange(
                    x,
                    entityName,
                    path,
                    x.LineNumber,
                    changeType,
                    fieldNames)
            ).ToArray();
    }

    private EntityChange ConvertLineToEntityChange(
        Line x, 
        string entityName,
        string path,
        int lineNumber, 
        EntityChangeTypeEnum changeType,
        string[] fieldNames) 
    {
        return new EntityChange(
            entityName,
            path,
            x.LineNumber,
            EntityChangeTypeEnum.Removed,
            OriginalFields: this.GetFieldData(fieldNames, x.Content));
    }

    private Patch? GetChangesBasedOnLastCommit(Repository repo) 
    {
        var latestCommit = repo.Head.Tip;

        // Get the previous commit
        var previousCommit = latestCommit.Parents.FirstOrDefault();

        // get diffs
        var allDiffs = repo.Diff.Compare<TreeChanges>(
            latestCommit.Tree,
            DiffTargets.Index | DiffTargets.WorkingDirectory);

        var diffs = allDiffs
            .Where(x => this.IsPathCorrect(x.Path))
            .ToArray();

        var pathes = diffs.Select(x => x.Path).ToArray();

        if (!pathes.Any()) 
        {
            return null;
        }

        var patches = repo.Diff.Compare<Patch>(
            previousCommit?.Tree,
            latestCommit.Tree,
            pathes);

        return patches;
    }

    private Patch? GetChangesBasedOnMainBranch(Repository repo) 
    {
        var mainBranch = repo.Branches[mainBranchName];

        var currentBranch = repo.Head;

        if(mainBranch == null || mainBranch.Tip == null || mainBranch.Tip.Tree == null)
        {
            Console.WriteLine("Main branch is empty");
            
            if(mainBranch == null)
            {
                Console.WriteLine("Main branch is null");
            }
            else if(mainBranch.Tip == null)
            {
                Console.WriteLine("Main branch tip is null");
            }
            else if(mainBranch.Tip.Tree == null)
            {
                Console.WriteLine("Main branch tip tree is null");
            }
        }

        if (currentBranch == null || currentBranch.Tip == null || currentBranch.Tip.Tree == null)
        {
            Console.WriteLine("Current branch is empty");
        }

        Console.WriteLine($"Main branch: {mainBranch.FriendlyName}");
        Console.WriteLine($"Current branch: {currentBranch.FriendlyName}");

        var allDiffs = repo.Diff.Compare<TreeChanges>(
            mainBranch?.Tip?.Tree,
            currentBranch?.Tip?.Tree);

        var diffs = allDiffs
            .Where(x => this.IsPathCorrect(x.Path))
            .ToArray();

        var pathes = diffs.Select(x => x.Path).ToArray();

        if(!pathes.Any())
        {
            return null;
        }

        var patches = repo.Diff.Compare<Patch>(
                mainBranch.Tip.Tree,
                currentBranch.Tip.Tree,
                pathes);

        return patches;
    }

    private bool IsPathCorrect(string path) => 
        path.StartsWith(dataFolderName) 
        && path.EndsWith(fileExtension);

    private string[] GetEntityFieldNames(string entityPath) 
    {
        var fullPath = Path.Combine(
                repoPath,
                entityPath);

        return File.ReadLines(fullPath)
            .First()
            .Split(',');
    }

    private Dictionary<string, string> GetFieldData(
        string[] fieldNames, string rowDataAsString) 
    {
        var rowData = rowDataAsString.Split(',');

        var result = new Dictionary<string, string>();

        for (int i = 0; i < fieldNames.Length; i++) 
        {
            var fieldName = fieldNames[i];
            var fieldValue = rowData.Length >= i 
                ? rowData[i]
                : string.Empty;

            result.Add(fieldName, fieldValue);
        }

        return result;
    }

    private string GetEntityName(PatchEntryChanges entityPatch)
    {
        return entityPatch.Path
            .Split("/")[1];
    }
}

public enum GetChangesModeEnum
{
    LastCommit,
    MainBranch
}
