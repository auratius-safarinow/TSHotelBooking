public class BookingIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BookingIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateBooking_ShouldReturnSuccess()
    {
        var request = new
        {
            HotelId = 1,
            CheckInDate = "2025-06-01",
            CheckOutDate = "2025-06-05",
            NumberOfGuests = 2,
            GuestName = "John Doe",
            GuestEmail = "john.doe@example.com"
        };

        var response = await _client.PostAsJsonAsync("/api/bookings", request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Booking created successfully", content);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnBookingDetails()
    {
        var response = await _client.GetAsync("/api/bookings/d01b2f7c-c95a-4fd6-bb79-d72d0e5ed575");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("John Doe", content);
    }

    [Fact]
    public async Task CreateBooking_InvalidInput_ShouldReturn400()
    {
        var request = new
        {
            HotelId = 1,
            CheckInDate = "2025-06-10",
            CheckOutDate = "2025-06-05", // Invalid date
            NumberOfGuests = 0, // Invalid guest count
            GuestName = "John Doe",
            GuestEmail = "john.doe@example.com"
        };
    
        var response = await _client.PostAsJsonAsync("/api/bookings", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateBooking_ServerError_ShouldReturn500()
    {
        // Simulate exception in the service layer
        var mockService = new Mock<IBookingService>();
        mockService.Setup(s => s.CreateBookingAsync(It.IsAny<BookingRequestDto>()))
            .ThrowsAsync(new Exception("Test exception"));
    
        var controller = new BookingsController(mockService.Object);
        
        var result = await controller.CreateBooking(new BookingRequestDto());
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
    
}
