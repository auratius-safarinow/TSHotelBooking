# TSHotelBooking

TSHotelBooking is a simple hotel booking API that simulates hotel bookings. The application allows users to create bookings, check availability, and retrieve booking details using an in-memory database. It also validates booking data such as check-in/check-out dates, the number of guests, and hotel existence.

## Features

- **Create a Booking**: Users can create hotel bookings by submitting booking details.
- **Retrieve Booking Details**: Users can retrieve the details of a booking using a unique reference ID.
- **Availability Simulation**: Availability is simulated for certain conditions (e.g., hotels are unavailable in December).
- **In-Memory Database**: All bookings are stored in an in-memory list for persistence across requests.
- **Validation**: Ensures proper check-in/check-out dates, valid guest counts, and valid hotel IDs.

## Folder Structure

```plaintext
TSHotelBooking/
TSHotelBooking.sln
│
├── TSHotelBooking.API
│   ├── Controllers
│   │   └── BookingsController.cs
|   ├── Middlewares
│   │   └── ExceptionMiddleware.cs
│   └── Program.cs
│   
├── TSHotelBooking.Application
│   ├── DTOs
│   │   ├── BookingDetailsDto.cs
│   │   └── BookingRequestDto.cs
│   ├── Contracts
│   │   └── IBookingService.cs
│   ├── BookingService.cs
│   └── TSHotelBooking.Application.csproj
│
├── TSHotelBooking.Domain
│   ├── Entities
│   │   ├── Booking.cs
│   │   └── Hotel.cs
│   ├── Contracts
│   │   ├── IBookingRepository.cs
│   │   └── IHotelRepository.cs
│   ├── Common
│   │   └── ServiceResult.cs
│   └── TSHotelBooking.Domain.csproj
│
├── TSHotelBooking.Infrastructure
│   ├── Implementations
│   │   ├── BookingRepository.cs
│   │   └── HotelRepository.cs
│   ├── Data
│   │   └── InMemoryDatabase.cs
│   └── TSHotelBooking.Infrastructure.csproj
└── TSHotelBooking.Test
|   ├── Services
|   |   └── BookingServiceTests.cs
|   └── TSHotelBooking.Tests.csproj

```

## Setup

### Prerequisites

- **.NET SDK 8.0** or higher.
- A **code editor** like Visual Studio or Visual Studio Code.
- **Git** to clone the repository.

### Steps to Build and Run

1. **Clone the repository**:
   Clone the project to your local machine using Git. Open your terminal and run:

   Clone the repository:
   `git clone https://github.com/krysremix/TSHotelBooking.git`
   `cd TSHotelBooking`

2. **Restore dependencies**:
   In case the dependencies are not automatically restored, run the following command in the terminal:

   `dotnet restore`

3. **Build the application**:
   Open the project in your code editor (Visual Studio, VS Code, or another editor), or build directly from the terminal:

   `dotnet build`

4. **Run the application**:
   Once the project is built, you can run it with the following command in the terminal:

   `dotnet run`

   This will start the API on `http://localhost:5000` (or a different port depending on your configuration).

5. **Test the API**:
   You can now test the API using an API testing tool (like [Postman](https://www.postman.com/)) or `curl`.

   **Example cURL request to create a booking**:

   `curl -X POST "http://localhost:5000/api/bookings" -H "Content-Type: application/json" -d '{
     "HotelId": "H001",
     "CheckInDate": "2025-06-01",
     "CheckOutDate": "2025-06-05",
     "NumberOfGuests": 2,
     "GuestName": "John Doe",
     "GuestEmail": "john.doe@example.com"
   }'`

   **Example cURL request to retrieve a booking by reference**:

   `curl -X GET "http://localhost:5000/api/bookings/d01b2f7c-c95a-4fd6-bb79-d72d0e5ed575"`

### API Documentation

For the detailed API documentation, check out the available endpoints listed below.

---

## Available Endpoints

### `POST /api/bookings`

Create a booking.

**Request body**:

`{ "HotelId": "H001", "CheckInDate": "2025-06-01", "CheckOutDate": "2025-06-05", "NumberOfGuests": 2, "GuestName": "John Doe", "GuestEmail": "john.doe@example.com" }`

**Response**:

`{ "data": "d01b2f7c-c95a-4fd6-bb79-d72d0e5ed575", "success": true, "message": "Booking created successfully" }`

### `GET /api/bookings/{reference}`

Retrieve booking details by reference.

**Request**:

`GET /api/bookings/d01b2f7c-c95a-4fd6-bb79-d72d0e5ed575`

**Response**:

`{ "guestName": "John Doe", "numberOfGuests": 2, "hotelId": "H001", "checkInDate": "2025-06-01T00:00:00", "checkOutDate": "2025-06-05T00:00:00" }`

---

## Error Handling

- **400 Bad Request**: If the request is invalid (e.g., missing required fields, invalid dates, etc.).
- **404 Not Found**: If the hotel is not found or the booking reference does not exist.
- **409 Conflict**: If there is an availability conflict (e.g., the hotel is unavailable in December).
- **500 Internal Server Error**: If an unexpected error occurs (handled globally by middleware).

## Design Decisions

- **Service Layer**: A service layer (`BookingService`) is used to separate business logic from the controller, making the code easier to maintain and test.
- **Repository Pattern**: The repository pattern is implemented with an in-memory repository (`BookingRepository`), allowing easy changes to the data source in the future (e.g., to a real database).
- **Middleware for Global Error Handling**: A custom middleware (`ExceptionMiddleware`) is used to handle exceptions globally, making the error handling more consistent across the application.
- **DTOs**: Data Transfer Objects (DTOs) are used to define the input and output structures for the API endpoints, ensuring a clear separation between the internal model and the API contract.

---
