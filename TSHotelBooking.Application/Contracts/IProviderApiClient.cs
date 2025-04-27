using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSHotelBooking.Application.DTOs;

namespace TSHotelBooking.Application.Contracts
{
    public interface IProviderApiClient
    {
        Task<ProviderAvailabilityResponse> GetAvailabilityAsync(int hotelId);
    }
}
