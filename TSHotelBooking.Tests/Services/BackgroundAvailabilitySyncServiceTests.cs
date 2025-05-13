public class BackgroundAvailabilitySyncServiceTests
{
    [Fact]
    public async Task SyncAvailability_ShouldLogSuccess()
    {
        var mockApiClient = new Mock<IProviderApiClient>();
        mockApiClient.Setup(client => client.GetAvailabilityAsync(It.IsAny<int>()))
            .ReturnsAsync(new ProviderAvailabilityResponse
            {
                HotelId = 1,
                AvailableRooms = 5,
                LastUpdatedTimestamp = DateTime.UtcNow
            });

        var mockLogger = new Mock<ILogger<BackgroundAvailabilitySyncService>>();
        var service = new BackgroundAvailabilitySyncService(mockLogger.Object, mockApiClient.Object);

        await service.StartAsync(CancellationToken.None);
        mockLogger.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
    }
}
