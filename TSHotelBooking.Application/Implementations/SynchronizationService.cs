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

        public SynchronizationService(IProviderApiClient providerApiClient, IHotelRepository hotelRepository)
        {
            _providerApiClient = providerApiClient;
            _hotelRepository = hotelRepository;
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
                    Console.WriteLine($"Fetched availability for {hotelId}");
                }
                else
                {
                    Console.WriteLine($"No availability found for {hotelId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching availability for {hotelId}: {ex.Message}");
            }
        }
    }
}
