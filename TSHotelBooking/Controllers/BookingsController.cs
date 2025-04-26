using Microsoft.AspNetCore.Mvc;
using TSHotelBooking.Application.Contracts;
using TSHotelBooking.Application.DTOs;

namespace TSHotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequestDto request)
        {
            var result = await _bookingService.CreateBookingAsync(request);

            if (!result.Success)
                return BadRequest(new { error = result.Message });

            return CreatedAtAction(nameof(GetBookingByReference), new { reference = result.Data }, new { reference = result.Data });
        }

        [HttpGet]
        [Route("{reference}")]
        public async Task<IActionResult> GetBookingByReference(Guid reference)
        {
            var result = await _bookingService.GetBookingByReferenceAsync(reference);

            if (!result.Success)
                return StatusCode(result.StatusCode, new { error = result.Message });

            // Map your result to BookingDetailsDto
            var bookingDetails = new BookingDetailsDto
            {
                GuestName = result.Data.GuestName,
                GuestEmail = result.Data.GuestEmail,
                NumberOfGuests = result.Data.NumberOfGuests,
                HotelId = result.Data.HotelId,
                HotelName = result.Data.HotelName,
                CheckInDate = result.Data.CheckInDate,
                CheckOutDate = result.Data.CheckOutDate
            };

            return Ok(bookingDetails);
        }
    }
}
