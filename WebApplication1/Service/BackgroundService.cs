public class BackgroundService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceProvider _services;
    private readonly ILogger<BackgroundService> _logger;

    public BackgroundService(IServiceProvider services, ILogger<BackgroundService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background service is starting.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        _logger.LogInformation("Background service is working.");

        using (var scope = _services.CreateScope())
        {
            var userDownloadService = scope.ServiceProvider.GetRequiredService<UserDownloadService>();
            userDownloadService.FetchAndUpdateUsersAsync().GetAwaiter().GetResult();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
