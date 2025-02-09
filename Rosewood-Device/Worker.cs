using RosewoodDevice.Services.Interfaces;

namespace RosewoodDevice;

public class Worker(ILogger<Worker> logger, ISpeakerService speakerService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        speakerService.MonitorSpeakers();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}