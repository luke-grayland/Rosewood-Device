using Microsoft.AspNetCore.SignalR.Client;
using RosewoodDevice.Constants;
using RosewoodDevice.Services.Interfaces;

namespace RosewoodDevice.Services;

public class SpeakerService(HubConnection hubConnection) : ISpeakerService
{
    public void MonitorSpeakers()
    {
        Task.Run(async () =>
        {
            for (var i = 0; i < 3; i++)
            {
                await Task.Delay(1000);
                Console.WriteLine(i);
                await hubConnection.InvokeAsync(HubMethods.SendDeviceData, "Pi123", i.ToString());

                if (i == 2) i = 0;
            }
        });
    }
}