using Cocona;
using ManagedDb.Core;
using ManagedDb.Core.Features.GenerateDbs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ManagedDb.ConsoleClient.Commands;

public class CreateDbCommand
{
    private readonly DbGenerator dbGenerator;
    private readonly IOptions<ManagedDbOptions> options;
    private ILogger<CreateDbCommand> logger;

    public CreateDbCommand(
        DbGenerator dbGenerator,
        IOptions<ManagedDbOptions> options,
        ILogger<CreateDbCommand> logger)
    {
        this.dbGenerator = dbGenerator;
        this.options = options;
        this.logger = logger;
    }

    [Command("createdb")]
    public async Task Handle()
    {
        var pathToDataFolder = options.Value.DataFolderPath;
        var generatedDbPath = options.Value.DbPath;

        this.logger.LogInformation(
            "Generating db from data folder {0} to {1}", 
            pathToDataFolder, 
            generatedDbPath);

        this.dbGenerator.Generate(
            pathToDataFolder, 
            generatedDbPath);

        await Task.CompletedTask;
    }
}
