using Microsoft.AspNetCore.Mvc;
using Screenings.Api.Dtos;
using Screenings.Api.Services;

namespace Screenings.Api.Controllers
{
    [ApiController]
    [Route("api/screenings")]
    public sealed class ScreeningsController : ControllerBase
    {
        private readonly IScreeningService _service;

        public ScreeningsController(IScreeningService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateScreeningDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _service.CreateAsync(dto, cancellationToken);
            return Created($"/api/screenings/{id}", id);
        }
    }
}