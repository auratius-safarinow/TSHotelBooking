using Moq;
using TSHotelBooking.Application.DTOs;
using TSHotelBooking.Application.Implementations;
using TSHotelBooking.Domain.Entities;
using TSHotelBooking.Domain.Contracts;
using FluentAssertions;

namespace TSHotelBooking.Tests.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly BookingService _bookingService;

        public BookingServiceTests()
        {
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();

            _bookingService = new BookingService(
                _hotelRepositoryMock.Object,
                _bookingRepositoryMock.Object
            );
        }

        [Fact]
        public async Task CreateBookingAsync_HotelNotFound_Returns404()
        {
            // Arrange
            var request = new BookingRequestDto
            {
                HotelId = 999,
                CheckInDate = DateTime.Now.AddDays(1),
                CheckOutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 2,
                GuestName = "John Doe",
                GuestEmail = "john@example.com"
            };

            _hotelRepositoryMock.Setup(repo => repo.GetHotelByIdAsync(It.IsAny<int>()))
                                .ReturnsAsync((Hotel)null);

            // Act
            var result = await _bookingService.CreateBookingAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(404);
            result.Message.Should().Be("Hotel not found.");
        }

        [Fact]
        public async Task CreateBookingAsync_HotelUnavailableInDecember_Returns409()
        {
            // Arrange
            var request = new BookingRequestDto
            {
                HotelId = 1,
                CheckInDate = new DateTime(2025, 12, 5),
                CheckOutDate = new DateTime(2025, 12, 10),
                NumberOfGuests = 2,
                GuestName = "John Doe",
                GuestEmail = "john@example.com"
            };

            _hotelRepositoryMock.Setup(repo => repo.GetHotelByIdAsync(request.HotelId))
                                .ReturnsAsync(new Hotel { Id = 1, Name = "Test Hotel" });

            // Act
            var result = await _bookingService.CreateBookingAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(409);
            result.Message.Should().Be("Hotel is unavailable in December.");
        }

        [Fact]
        public async Task CreateBookingAsync_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new BookingRequestDto
            {
                HotelId = 1,
                CheckInDate = new DateTime(2025, 11, 10),
                CheckOutDate = new DateTime(2025, 11, 15),
                NumberOfGuests = 2,
                GuestName = "Jane Doe",
                GuestEmail = "jane@example.com"
            };

            _hotelRepositoryMock.Setup(repo => repo.GetHotelByIdAsync(request.HotelId))
                                .ReturnsAsync(new Hotel { Id = 1, Name = "Test Hotel" });

            _bookingRepositoryMock.Setup(repo => repo.CreateBookingAsync(It.IsAny<Booking>()))
                                  .Returns(Task.CompletedTask);

            // Act
            var result = await _bookingService.CreateBookingAsync(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeEmpty();
        }
    }
}
