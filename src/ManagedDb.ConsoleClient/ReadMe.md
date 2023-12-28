# ManagedDb.ConsoleClinet

## How to run locally

All params come from configs. 

To change params you need to go `properties/launchSettings.json` file and provide:

```json
{
  "profiles": {
    "ManagedDb.ConsoleClient": {
      "commandName": "Project",
      "commandLineArgs": "",
      "environmentVariables": {
        "MDBRootFolder": "$(MDBRootFolder)",
        "ManagedDb__PrId": "<YOUR-PR-ID>",
        "ManagedDb__Token": "<YOUR-GITHUB-API-TOKEN>",
        "ManagedDb__PathToSave": "$(DataOutputPath)",
        "ManagedDb__RepoPath": "$(MDBRootFolder)"
      }
    }
  }
}
```

Don't worry, this file is ingored by git, so no your data will be saved with PR

## How to use in CI/CD

Technically the same. 

Please find a release pipeline in the repo for data processing. 

In order to provide data to the console app, we just need to replare `appsettings.json` file with proper valies

## Testing

More or less we have tests for `Core`. 