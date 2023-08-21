using Microsoft.Extensions.Configuration;

namespace WorkerService_Demo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service started.");

            while (true)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                Task.Delay(1000, cancellationToken).Wait();
            }
            return Task.CompletedTask;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                string Filelocation = _configuration.GetValue<string>("Filepath");
                string Filename = _configuration.GetValue<string>("Filename");
                Createfile(Filelocation, Filename);
                await Task.Delay(5000,stoppingToken);

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service stopped.");
            return Task.CompletedTask;

        }
        private void Createfile(string Filelocation, string Filename)
        {
            var file = Filelocation + "\\" + Filename;
            if (!File.Exists(file))
            {
                for (int i = 0; i <= 100; i++)
                {
                    var Fname = Filelocation + "\\" + "Workerfile_" + i + ".txt";
                    File.Create(Fname);
                    _logger.LogInformation("File created:{Fname} successfully", Fname);
                }
            }
        }

    }
}