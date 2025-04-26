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
    public class BookingRepository : IBookingRepository
    {
        public async Task CreateBookingAsync(Booking booking)
        {
            InMemoryDatabase.Bookings.Add(booking);
            await Task.CompletedTask;
        }

        public async Task<Booking> GetBookingByReferenceAsync(Guid reference)
        {
            // Fetch the booking from the static in-memory list
            var booking = InMemoryDatabase.Bookings.FirstOrDefault(b => b.BookingReference == reference);
            return await Task.FromResult(booking); // Simulate async operation
        }
    }
}
