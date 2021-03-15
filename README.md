See the following for further info about .NET 5 Functions:<br />
[Official documentation](https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide)

[GitHub repository for Functions DotNet Worker](https://github.com/Azure/azure-functions-dotnet-worker)

This code primarily demonstrates very basic examples of using dependency injection in .NET 5 Functions (see the Program.cs and Function1.cs), to illustrate how it compares with DI in .NET Core 2.x/3.x Functions.

BlobTriggerGetMetaData.cs demonstrates an example of how to get trigger metadata in .NET 5 Functions, and also shows an example of how to do multiple output bindings.
