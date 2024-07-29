public class BackgroundService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceProvider _services;
    private readonly ILogger<BackgroundService> _logger;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="services"></param>
    /// <param name="logger"></param>
    public BackgroundService(IServiceProvider services, ILogger<BackgroundService> logger)
    {
        _services = services;
        _logger = logger;
    }

    /// <summary>
    /// Metodo que llama el servicio cuando se inicia
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("El servicio en segundo plano se está inciando.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Metodo que inicia el servicio
    /// </summary>
    /// <param name="state"></param>
    private void DoWork(object state)
    {
        _logger.LogInformation("El servicio en segundo plano está en marcha.");

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
