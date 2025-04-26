using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSHotelBooking.Domain.Entities;

namespace TSHotelBooking.Domain.Contracts
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetHotelByIdAsync(int hotelId);
    }
}
