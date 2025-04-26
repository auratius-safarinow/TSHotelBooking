using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSHotelBooking.Application.DTOs;
using TSHotelBooking.Domain.Common;
using TSHotelBooking.Domain.Entities;

namespace TSHotelBooking.Application.Contracts
{
    public interface IBookingService
    {
        Task<ServiceResult<Guid>> CreateBookingAsync(BookingRequestDto request);
        Task<ServiceResult<Booking>> GetBookingByReferenceAsync(Guid reference);
    }
}
