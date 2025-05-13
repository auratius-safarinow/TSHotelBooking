1. System Architecture Diagram
Description:
This diagram shows the overall architecture of the TSHotelBooking system, including its layers: API, Application, Domain, and Infrastructure.

+------------------------------------------+
|                API Layer                 |
|  - AvailabilityController.cs             |
|  - BookingsController.cs                 |
+------------------------------------------+
                  |
                  v
+------------------------------------------+
|             Application Layer            |
|  - BookingService.cs                     |
|  - ProviderApiClient.cs                  |
|  - SynchronizationService.cs             |
+------------------------------------------+
                  |
                  v
+------------------------------------------+
|                Domain Layer              |
|  - Booking.cs                            |
|  - Hotel.cs                              |
|  - ServiceResult.cs                      |
+------------------------------------------+
                  |
                  v
+------------------------------------------+
|            Infrastructure Layer          |
|  - BookingRepository.cs                  |
|  - HotelRepository.cs                    |
+------------------------------------------+

2. Booking Flow Diagram
Description:
This diagram illustrates the flow of creating a booking, from the user making a request to the system storing the booking details.

User
  |
  v
[Booking Request]
  |
  v
AvailabilityController.cs
  |
  v
BookingService.cs
  |
  v
InMemoryDatabase.cs
  |
  v
[Booking Stored]

3. Sequence Diagram: Create Booking
Description:
This diagram represents the sequence of interactions for creating a booking.

User -> API: Submit booking request
API -> Application: Validate request
Application -> Domain: Create booking object
Domain -> Infrastructure: Store booking in memory
Infrastructure -> Domain: Return success
Domain -> Application: Return booking reference
Application -> API: Send response to user

4. Background Service Workflow
Description:
This diagram shows the workflow of the BackgroundAvailabilitySyncService.

+-------------------------------+
| BackgroundAvailabilitySyncService |
+-------------------------------+
                |
                v
     Fetch availability data
                |
                v
     Update in-memory cache
                |
                v
        Log success/failure
                |
                v
     Wait for next sync (5 mins)


