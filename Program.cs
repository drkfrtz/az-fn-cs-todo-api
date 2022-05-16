using Azure.Core.Serialization;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using TodoApi.Services.Repositories;

namespace TodoApi
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureOpenApi()
                .ConfigureServices(
                    s =>
                    {
                        s.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
                        // s.AddSingleton<TableServiceClient>(
                        //     new TableServiceClient(
                        //         connectionString: Environment.GetEnvironmentVariable(
                        //             "AzureWebJobsStorage"
                        //         )
                        //     )
                        // );
                        // s.AddSingleton<ITodoRepository, TableStorageTodoRepository>(
                        //     factory =>
                        //     {
                        //         var tableName = "TodosTable";
                        //         var tableServiceClient =
                        //             factory.GetRequiredService<TableServiceClient>();
                        //         tableServiceClient.CreateTableIfNotExists(tableName);
                        //         return new TableStorageTodoRepository(
                        //             tableServiceClient: tableServiceClient,
                        //             tableClient: new TableClient(
                        //                 connectionString: Environment.GetEnvironmentVariable(
                        //                     "AzureWebJobsStorage"
                        //                 ),
                        //                 tableName: tableName
                        //             )
                        //         );
                        //     }
                        // );
                        s.AddSingleton<ObjectSerializer>(
                            new JsonObjectSerializer(
                                new JsonSerializerOptions
                                {
                                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                    WriteIndented = true,
                                    DefaultIgnoreCondition = System
                                        .Text
                                        .Json
                                        .Serialization
                                        .JsonIgnoreCondition
                                        .WhenWritingNull
                                }
                            )
                        );
                    }
                )
                .Build();

            host.Run();
        }
    }
}
