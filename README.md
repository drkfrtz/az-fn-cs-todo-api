# Todo Api with Azure Functions, C# & .NET 6.0

This is a simple demo application of how one could create a trivial REST Api with Azure Functions in C# and .NET 6.0.

## Development Setup

You'll need the following tools:

- [VSCode](https://code.visualstudio.com/)
- [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Azure Functions Core Tools v4](https://github.com/Azure/azure-functions-core-tools/releases)
- [Azurite](https://github.com/Azure/Azurite/releases) or [Azurite VSCode Plugin](https://marketplace.visualstudio.com/items?itemName=Azurite.azurite)
- [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)

If you're on MacOS (like me):

```bash
brew install mono-libgdiplus
```

## Getting started

Create a `local.settings.json` file in the root folder.

```bash
touch local.settings.json
```

Paste the following lines:

```json
{
	"IsEncrypted": false,
	"Values": {
		"AzureWebJobsStorage": "UseDevelopmentStorage=true",
		"FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
	}
}
```

Run Azurite locally ...

... from VSCode:

`> Azurite: Start Table Service`

... from console:

```bash
azurite-table -l path/to/azurite/workspace
```

Run the Azure Functions App locally from console:

```bash
func start
```
