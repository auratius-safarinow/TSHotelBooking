using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSHotelBooking.Application.DTOs
{
    public class BookingDetailsDto
    {
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public int NumberOfGuests { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
