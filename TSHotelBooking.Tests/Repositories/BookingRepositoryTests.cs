public class BookingRepositoryTests
{
    [Fact]
    public async Task CreateBooking_ShouldAddToDatabase()
    {
        var booking = new Booking
        {
            BookingReference = Guid.NewGuid(),
            GuestName = "Jane Doe",
            GuestEmail = "jane.doe@example.com"
        };

        var repository = new BookingRepository();
        await repository.CreateBookingAsync(booking);

        var fetchedBooking = await repository.GetBookingByReferenceAsync(booking.BookingReference);
        Assert.NotNull(fetchedBooking);
        Assert.Equal("Jane Doe", fetchedBooking.GuestName);
    }

    [Fact]
    public void ValidateBooking_InvalidDates_ShouldReturnFailure()
    {
        var request = new BookingRequestDto
        {
            CheckInDate = DateTime.Now.AddDays(5),
            CheckOutDate = DateTime.Now.AddDays(3) // Invalid
        };
    
        var result = BookingService.ValidateBooking(request);
        Assert.False(result.Success);
        Assert.Equal("Check-out date must be after check-in date.", result.Message);
    }

    [Fact]
    public void ValidateBooking_InvalidGuestCount_ShouldReturnFailure()
    {
        var request = new BookingRequestDto
        {
            NumberOfGuests = -1 // Invalid
        };
    
        var result = BookingService.ValidateBooking(request);
        Assert.False(result.Success);
        Assert.Equal("Number of guests must be greater than zero.", result.Message);
    }

    [Fact]
    public async Task ConcurrentBookingRequests_ShouldNotCauseConflicts()
    {
        var mockRepository = new Mock<IBookingRepository>();
        mockRepository.Setup(repo => repo.CreateBookingAsync(It.IsAny<Booking>()))
            .Returns(Task.CompletedTask);
    
        var service = new BookingService(new Mock<IHotelRepository>().Object, mockRepository.Object);
    
        var tasks = Enumerable.Range(0, 10).Select(async _ =>
        {
            var request = new BookingRequestDto
            {
                HotelId = 1,
                CheckInDate = DateTime.Now.AddDays(1),
                CheckOutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 1,
                GuestName = "Concurrent User",
                GuestEmail = "user@example.com"
            };
    
            var result = await service.CreateBookingAsync(request);
            Assert.True(result.Success);
        });
    
        await Task.WhenAll(tasks);
    }
}
