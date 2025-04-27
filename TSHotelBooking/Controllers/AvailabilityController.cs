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
        public ActionResult<IEnumerable<CachedAvailability>> GetAll()
        {
            var availabilities = _syncService.GetCachedAvailabilities();
            return Ok(availabilities);
        }
    }
}
