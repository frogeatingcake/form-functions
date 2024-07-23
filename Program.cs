using Form_Function_App;
using Form_Function_App.MappingProfiles;
using Form_Function_App.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        
        //services reg
        services.AddSingleton<SubmitDataService>();
        services.AddSingleton<QueueToTableService>();
        services.AddSingleton<SendDataToApiService>();
        services.AddSingleton<DataFromTableService>();
        services.AddSingleton<SendEmailService>();
        services.AddSingleton<RetryHandleService>();
        services.AddSingleton<ScheduledBatchService>();
        services.AddLogging();

        // httpclient reg
        services.AddHttpClient<SendDataToApiService>();

        // automapper reg
        services.AddAutoMapper(typeof(UserMappingProfile)); // Register AutoMapper with the UserProfile



    })
    .Build();

host.Run();
