using AdminPortal.Data;

namespace AdminPortal.Services
{
    public class SeedHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                await SeedData.Initialize(services);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
