# ManagedDb.Proxies

## Introduction

This library was created for customize client 'data model'. 

By default, if you open the project in VS and use "debug" mode, you should check if "models" and "controllers" created correctly with respect to sql lit db. 

In "release" mode, the project will generate models and controllers based on jsons from "App_Data" folder.

### Lib content

1. Set of types for models
2. Set of OData controllers

Models will be used for generating DbSets in runtime for EF.

Controllers will be used for reaching data from db.

The controllers support OData protocol.

## How to use

### Requirements

* [dotnet-t4](https://github.com/mono/t4). Install tool globally

We use `t4` for creating proxy classes in Release mode. 

In `csproj` file you can find a target which will execute t4 util. 

### Instraction

#### Debug mode

There are no any special requirements for using this library. Just add it to your project and use it.

#### Release mode

1. Get mdb schema json files
2. Add them to "App_Data" folder. Remember the extension of files should be `*.mdb.entity.schema.json`
3. Run `dotnet run -c Release`
4. Copy `ManagedDb.Proxies.dll` and `ManagedDb.Proxies.pdb` files
5. Past to `ManagedDb.WebApi` publish project folder

## How to test

Coming soon...