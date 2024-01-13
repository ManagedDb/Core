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

There are no any special requirements for using this library. Just add it to your project and use it.

For release mode, you should add json files to "App_Data" folder.

then build the project.

Extract the "ManagedDb.Proxies.dll" from "bin" folder and add it to your project (in real scenaio it will be webapp).

## How to test

Coming soon...