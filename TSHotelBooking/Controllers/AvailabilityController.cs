using Microsoft.AspNetCore.Mvc;
using TSHotelBooking.Application.DTOs;
using TSHotelBooking.Application.Implementations;

namespace TSHotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly BackgroundAvailabilitySyncService _syncService;

        public AvailabilityController(BackgroundAvailabilitySyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CachedAvailability>>> GetAll(CancellationToken cancellationToken)
        {
            var availabilities = await _syncService.GetCachedAvailabilities(cancellationToken);
            return Ok(availabilities);
        }
    }
}
