using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TSHotelBooking.Application.Contracts;
using TSHotelBooking.Application.DTOs;

namespace TSHotelBooking.Application.Implementations
{
    public class BackgroundAvailabilitySyncService : BackgroundService
    {
        private readonly ILogger<BackgroundAvailabilitySyncService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ConcurrentDictionary<string, CachedAvailability> _cache = new();
        private readonly List<int> _hotelIds;

        public BackgroundAvailabilitySyncService(
            ILogger<BackgroundAvailabilitySyncService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _hotelIds = GenerateDummyHotelIds(100);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting background hotel availability sync...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await SyncAvailabilityAsync(_hotelIds, stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Sync every 5 minutes
            }
        }

        private async Task SyncAvailabilityAsync(IEnumerable<int> hotelIds, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting sync for {Count} hotels...", hotelIds.Count());

            var successCount = 0;
            var failedCount = 0;

            var tasks = hotelIds.Select(async hotelId =>
            {
                // Create a scope to resolve scoped services
                using var scope = _serviceScopeFactory.CreateScope();
                var providerApiClient = scope.ServiceProvider.GetRequiredService<IProviderApiClient>();

                try
                {
                    var availability = await providerApiClient.GetAvailabilityAsync(hotelId);
                    _cache[hotelId.ToString()] = new CachedAvailability
                    {
                        HotelId = availability.HotelId,
                        AvailableRooms = availability.AvailableRooms,
                        PricePerNight = availability.PricePerNight,
                        LastUpdatedTimestamp = availability.LastUpdatedTimestamp
                    };

                    _logger.LogInformation("Fetched availability for hotel {HotelId}", hotelId);
                    Interlocked.Increment(ref successCount);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error fetching availability for hotel {HotelId}", hotelId);
                    Interlocked.Increment(ref failedCount);
                }
            });

            await Task.WhenAll(tasks);

            _logger.LogInformation("Sync completed. Success: {Success}, Failed: {Failed}", successCount, failedCount);
        }

        private List<int> GenerateDummyHotelIds(int count)
        {
            var list = new List<int>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(i);
            }
            return list;
        }

        public IEnumerable<CachedAvailability> GetCachedAvailabilities()
        {
            return _cache.Values.ToList();
        }

    }
}
