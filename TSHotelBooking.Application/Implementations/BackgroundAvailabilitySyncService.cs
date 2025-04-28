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
        private readonly IProviderApiClient _providerApiClient;
        private readonly ConcurrentDictionary<string, CachedAvailability> _cache = new();
        private readonly List<CachedAvailability> _hotelIds;
        private readonly Random _random = new();

        public BackgroundAvailabilitySyncService(
            ILogger<BackgroundAvailabilitySyncService> logger,
            IProviderApiClient providerApiClient)
        {
            _logger = logger;
            _providerApiClient = providerApiClient;
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

        private async Task SyncAvailabilityAsync(IEnumerable<CachedAvailability> hotelIds, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting sync for {Count} hotels...", hotelIds.Count());

            var successCount = 0;
            var failedCount = 0;

            var tasks = hotelIds.Select(hotelId => RetrieveHotelAvailabilityAsync(hotelId, successCount, failedCount));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Sync completed. Success: {Success}, Failed: {Failed}", successCount, failedCount);
        }

        private async Task<(int successCount, int failedCount)> RetrieveHotelAvailabilityAsync(CachedAvailability hotel, int successCount, int failedCount)
        {
            try
            {
                var availability = await _providerApiClient.GetAvailabilityAsync(hotel.HotelId);
                hotel.AvailableRooms = availability.AvailableRooms;
                hotel.LastUpdatedTimestamp = availability.LastUpdatedTimestamp;

                _cache[hotel.HotelId.ToString()] = hotel;

                _logger.LogInformation("Fetched availability for hotel {HotelId}", hotel);
                Interlocked.Increment(ref successCount);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching availability for hotel {HotelId}", hotel);
                Interlocked.Increment(ref failedCount);
            }

            return (successCount, failedCount);
        }

        private List<CachedAvailability> GenerateDummyHotelIds(int count)
        {
            var list = new List<CachedAvailability>();

            for (int i = 1; i <= count; i++)
            {
                list.Add(new CachedAvailability
                {
                    HotelId = i,
                    AvailableRooms = _random.Next(1, 300),
                    PricePerNight = Math.Round((decimal)(_random.NextDouble() * 100 + 50), 2, MidpointRounding.ToEven),
                    LastUpdatedTimestamp = DateTime.UtcNow
                });
            }
            return list;
        }

        public async Task<IEnumerable<CachedAvailability>> GetCachedAvailabilities(CancellationToken cancellationToken)
        {
            await SyncAvailabilityAsync(_hotelIds, cancellationToken);
            return _cache.Values;
        }

    }
}
