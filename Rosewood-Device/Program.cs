using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using RosewoodDevice;
using RosewoodDevice.Services;
using RosewoodDevice.Services.Interfaces;
using RosewoodDevice.Utilities;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("DeviceSettings"));
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<HubConnection>(provider =>
{
    try
    {
        var appSettings = provider.GetRequiredService<IOptions<AppSettings>>().Value;
        
        Console.WriteLine("Attempting to connect to hub");
        var connection = new HubConnectionBuilder()
            .WithUrl(appSettings.HubURL)
            .WithAutomaticReconnect()
            .Build();

        connection.Reconnecting += (error) =>
        {
            Console.WriteLine("Reconnecting...");
            return Task.CompletedTask;
        };

        connection.On<string>("ReceiveCommand", (command) =>
        {
            Console.WriteLine($"Command received: {command}");
        });
        
        connection.StartAsync();
        Console.WriteLine("Connected to hub");
        return connection;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed to connect to hub: {e.Message}");
        throw;
    }
});

builder.Services.AddSingleton<ISpeakerService, SpeakerService>();

var host = builder.Build();
host.Run();