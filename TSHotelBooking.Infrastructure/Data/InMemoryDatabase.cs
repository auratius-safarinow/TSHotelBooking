using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSHotelBooking.Domain.Entities;

namespace TSHotelBooking.Infrastructure.Data
{
    public static class InMemoryDatabase
    {
        public static List<Hotel> Hotels { get; set; } = new List<Hotel>
        {
            new Hotel { Id = 1, Name = "Grand Plaza" },
            new Hotel { Id = 2, Name = "Sea Breeze Resort" }
        };

        public static List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
