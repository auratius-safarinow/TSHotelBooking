using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSHotelBooking.Domain.Contracts;
using TSHotelBooking.Domain.Entities;
using TSHotelBooking.Infrastructure.Data;

namespace TSHotelBooking.Infrastructure.Implementations
{
    public class HotelRepository : IHotelRepository
    {
        public async Task<Hotel?> GetHotelByIdAsync(int hotelId)
        {
            var hotel = InMemoryDatabase.Hotels.FirstOrDefault(h => h.Id == hotelId);
            return await Task.FromResult(hotel);
        }
    }
}
