using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSHotelBooking.Application.Contracts;
using TSHotelBooking.Application.DTOs;

namespace TSHotelBooking.Application.Implementations
{
    public class ProviderApiClient : IProviderApiClient
    {
        private readonly Random _random = new();

        public async Task<ProviderAvailabilityResponse> GetAvailabilityAsync(int hotelId)
        {
            // Simulate network delay
            await Task.Delay(TimeSpan.FromMilliseconds(_random.Next(50, 300)));

            // Simulate occasional failures
            if (_random.NextDouble() < 0.1) // 10% chance to throw error
            {
                throw new HttpRequestException("Simulated network error");
            }

            // Return fake availability data
            return new ProviderAvailabilityResponse
            {
                HotelId = hotelId,
                AvailableRooms = _random.Next(0, 10),
                LastUpdatedTimestamp = DateTime.UtcNow
            };
        }
    }
}
