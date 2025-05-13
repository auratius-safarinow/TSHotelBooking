# TSHotelBooking

TSHotelBooking is a simple hotel booking API that simulates hotel bookings. The application allows users to create bookings, check availability, and retrieve booking details using an in-memory database. It also validates booking data such as check-in/check-out dates, the number of guests, and hotel existence. A background service periodically syncs hotel availability from a simulated third-party API.

## Features

- **Create a Booking**: Users can create hotel bookings by submitting booking details.
- **Retrieve Booking Details**: Users can retrieve the details of a booking using a unique reference ID.
- **Availability Simulation**: Availability is simulated for certain conditions (e.g., hotels are unavailable in December).
- **In-Memory Database**: All bookings are stored in an in-memory list for persistence across requests.
- **Validation**: Ensures proper check-in/check-out dates, valid guest counts, and valid hotel IDs.
- **Background Sync**: A hosted background service syncs availability data every 5 minutes.

## Folder Structure

TSHotelBooking/
TSHotelBooking.sln

- **TSHotelBooking.API**
  - Controllers/
    - AvailabilityController.cs 
    - BookingsController.cs
  - Middlewares/
    - ExceptionMiddleware.cs
  - Program.cs

- **TSHotelBooking.Application**
  - DTOs/
    - BookingDetailsDto.cs
    - BookingRequestDto.cs
    - CachedAvailability.cs
    - ProviderAvailabilityResponse.cs
  - Contracts/
    - IBookingService.cs
    - IProviderApiClient.cs
    - ISynchronizationService.cs
  - Implementations/
    - BookingService.cs
    - BackgroundAvailabilitySyncService.cs
    - ProviderApiClient.cs
    - SynchronizationService.cs
  - TSHotelBooking.Application.csproj

- **TSHotelBooking.Domain**
  - Entities/
    - Booking.cs
    - Hotel.cs
  - Contracts/
    - IBookingRepository.cs
    - IHotelRepository.cs
  - Common/
    - ServiceResult.cs
  - TSHotelBooking.Domain.csproj

- **TSHotelBooking.Infrastructure**
  - Implementations/
    - BookingRepository.cs
    - HotelRepository.cs
  - Data/
    - InMemoryDatabase.cs
  - TSHotelBooking.Infrastructure.csproj

- **TSHotelBooking.Test**
  - Services/
    - BookingServiceTests.cs
  - TSHotelBooking.Tests.csproj

## Setup

### Prerequisites

- .NET SDK 8.0 or higher
- Visual Studio or VS Code
- Git

### Steps

1. Clone the repository:
   git clone https://github.com/krysremix/TSHotelBooking.git
   cd TSHotelBooking

2. Restore dependencies:
   dotnet restore

3. Build the application:
   dotnet build

4. Run the application:
   dotnet run

   The API will start at http://localhost:5000 or another configured port.

5. Test with curl or Postman.

## Design Decisions

- **Layered Architecture**: The project uses a clean architecture with distinct layers for API, Application, Domain, and Infrastructure.
- **In-Memory Database**: Selected for simplicity and performance during prototyping.
- **Background Service**: A hosted service handles synchronization with external providers every 5 minutes to ensure up-to-date availability data.
- **Error Handling**: Centralized error handling middleware to return consistent API responses.

## Assumptions

- Hotel bookings are stored in memory and not persisted beyond the lifecycle of the application.
- Availability is simulated with predefined rules (e.g., no availability in December).

## Limitations

- The in-memory database is not suitable for production use.
- The project currently lacks integration with a real database or external services.

### Example Request (Create Booking)

curl -X POST "http://localhost:5000/api/bookings" -H "Content-Type: application/json" -d '{
  "HotelId": 1,
  "CheckInDate": "2025-06-01",
  "CheckOutDate": "2025-06-05",
  "NumberOfGuests": 2,
  "GuestName": "John Doe",
  "GuestEmail": "john.doe@example.com"
}'

### Example Request (Get Booking by Reference)

curl -X GET "http://localhost:5000/api/bookings/d01b2f7c-c95a-4fd6-bb79-d72d0e5ed575"

## API Endpoints

### POST /api/bookings
Create a booking.

**Response:**
{ "data": "<reference-id>", "success": true, "message": "Booking created successfully" }

### GET /api/bookings/{reference}
Get booking by reference.

**Response:**
{
  "guestName": "John Doe",
  "numberOfGuests": 2,
  "hotelId": 1,
  "hotelName": "Grand Plaza",
  "checkInDate": "2025-06-01T00:00:00",
  "checkOutDate": "2025-06-05T00:00:00"
}

## Background Service: Availability Sync

The `BackgroundAvailabilitySyncService` is a hosted service that runs every 5 minutes to sync hotel availability from a mock provider.

- Caches availability in memory using `ConcurrentDictionary`.
- Simulates external API latency and errors.
- Logs success/failure per hotel.

Registered in `Program.cs` via:

builder.Services.AddSingleton<IProviderApiClient, ProviderApiClient>();
builder.Services.AddHostedService<BackgroundAvailabilitySyncService>();

## Error Handling

- **400 Bad Request**: Validation failure
- **404 Not Found**: Invalid booking or hotel
- **409 Conflict**: Availability conflict (e.g. December blackout)
- **500 Internal Server Error**: Unexpected exception (caught globally)

## Design Highlights

- **Service Layer**: Keeps logic clean and testable
- **Repository Pattern**: Enables easy swapping of data sources
- **DTOs**: Ensures clear API contracts
- **Middleware**: Centralized error handling
- **Hosted Service**: Scalable sync strategy
