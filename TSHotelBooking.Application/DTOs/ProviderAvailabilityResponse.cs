using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSHotelBooking.Application.DTOs
{
    public class ProviderAvailabilityResponse
    {
        public int HotelId { get; set; }
        public int AvailableRooms { get; set; }
        public decimal PricePerNight { get; set; }
        public DateTime LastUpdatedTimestamp { get; set; }
    }
}
