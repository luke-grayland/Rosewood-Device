using Microsoft.AspNetCore.SignalR.Client;
using RosewoodDevice;
using RosewoodDevice.Services;
using RosewoodDevice.Services.Interfaces;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<HubConnection>(provider =>
{
    try
    {
        Console.WriteLine("Attempting to connect to hub");
        var connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7181/DeviceHub")
            .WithAutomaticReconnect()
            .Build();

        connection.Reconnecting += (error) =>
        {
            Console.WriteLine("Reconnecting...");
            return Task.CompletedTask;
        };
                
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