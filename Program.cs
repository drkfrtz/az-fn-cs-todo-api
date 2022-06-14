using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Azure.Data.Tables;

using TodoApi.Services.Repositories;
using System;

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
                    injector =>
                    {
                        injector.Configure<JsonSerializerOptions>(
                            options =>
                            {
                                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                                options.Converters.Add(new JsonStringEnumConverter());
                                options.DefaultIgnoreCondition =
                                    JsonIgnoreCondition.WhenWritingNull;
                            }
                        );

                        // injector.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
                        injector.AddSingleton(
                            new TableServiceClient(
                                connectionString: Environment.GetEnvironmentVariable(
                                    "AzureWebJobsStorage"
                                )
                            )
                        );
                        injector.AddSingleton<ITodoRepository, TableStorageTodoRepository>(
                            factory =>
                            {
                                var tableName = "Todos";
                                var tableServiceClient =
                                    factory.GetRequiredService<TableServiceClient>();
                                tableServiceClient.CreateTableIfNotExists(tableName);

                                return new TableStorageTodoRepository(
                                    tableServiceClient: tableServiceClient
                                );
                            }
                        );
                    }
                )
                .Build();
            host.Run();
        }
    }
}
