using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSHotelBooking.Application.Contracts;
using TSHotelBooking.Application.DTOs;
using TSHotelBooking.Domain.Common;
using TSHotelBooking.Domain.Entities;
using TSHotelBooking.Domain.Contracts;

namespace TSHotelBooking.Application.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IHotelRepository hotelRepository, IBookingRepository bookingRepository)
        {
            _hotelRepository = hotelRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<ServiceResult<Guid>> CreateBookingAsync(BookingRequestDto request)
        {
            if (request.CheckOutDate <= request.CheckInDate)
                return ServiceResult<Guid>.Failure("Check-out date must be after check-in date.", 400);

            if (request.NumberOfGuests <= 0)
                return ServiceResult<Guid>.Failure("Number of guests must be greater than zero.", 400);

            var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId);
            if (hotel == null)
                return ServiceResult<Guid>.Failure("Hotel not found.", 404);

            // Optional: Simulate unavailable dates (e.g., December is blocked)
            if (request.CheckInDate.Month == 12 || request.CheckOutDate.Month == 12)
                return ServiceResult<Guid>.Failure("Hotel is unavailable in December.", 409);

            var booking = new Booking
            {
                HotelId = request.HotelId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfGuests = request.NumberOfGuests,
                GuestName = request.GuestName,
                GuestEmail = request.GuestEmail,
                HotelName = hotel.Name
            };

            await _bookingRepository.CreateBookingAsync(booking);

            return ServiceResult<Guid>.Ok(booking.BookingReference);
        }

        public async Task<ServiceResult<Booking>> GetBookingByReferenceAsync(Guid reference)
        {
            var booking = await _bookingRepository.GetBookingByReferenceAsync(reference);

            if (booking == null)
                return ServiceResult<Booking>.Failure("Booking not found.", 404);

            return ServiceResult<Booking>.Ok(booking);
        }
    }
}
