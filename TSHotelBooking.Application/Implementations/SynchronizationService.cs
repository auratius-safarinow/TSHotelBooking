using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSHotelBooking.Application.Contracts;
using TSHotelBooking.Domain.Contracts;

namespace TSHotelBooking.Application.Implementations
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly IProviderApiClient _providerApiClient;
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger<SynchronizationService> _logger;

        public SynchronizationService(IProviderApiClient providerApiClient, IHotelRepository hotelRepository, ILogger<SynchronizationService> logger)
        {
            _providerApiClient = providerApiClient;
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        public async Task SyncAvailabilityAsync(IEnumerable<int> hotelIdsToUpdate)
        {
            var tasks = hotelIdsToUpdate.Select(hotelId => UpdateHotelAvailability(hotelId));
            await Task.WhenAll(tasks);
        }

        private async Task UpdateHotelAvailability(int hotelId)
        {
            try
            {
                var availability = await _providerApiClient.GetAvailabilityAsync(hotelId);
                if (availability != null)
                {
                    // Update local cache (or database)
                    _logger.LogInformation("Fetched availability for hotel {HotelId}", hotelId);
                }
                else
                {
                    _logger.LogWarning("No availability found for hotel {HotelId}", hotelId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching availability for hotel {HotelId}", hotelId);
            }
        }

        private async Task UpdateHotelAvailabilityWithRetry(int hotelId)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(exception, "Retry {RetryCount} for hotel {HotelId} failed.", retryCount, hotelId);
                    });
        
            await retryPolicy.ExecuteAsync(() => UpdateHotelAvailability(hotelId));
        }
    }
}
