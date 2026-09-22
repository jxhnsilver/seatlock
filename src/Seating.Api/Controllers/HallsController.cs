using Microsoft.AspNetCore.Mvc;
using Seating.Api.Dtos;
using Seating.Api.Services;

namespace Seating.Api.Controllers
{
    [Route("api/halls")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly IHallService _service;

        public HallsController(IHallService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<HallDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var hallDto = await _service.GetByIdAsync(id, cancellationToken);
            return Ok(hallDto);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<HallDto>>> GetAll(CancellationToken cancellationToken)
        {
            var hallDtos = await _service.GetAllAsync(cancellationToken);
            return Ok(hallDtos);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IReadOnlyList<HallDto>>> GetActive(CancellationToken cancellationToken)
        {
            var hallDtos = await _service.GetActiveAsync(cancellationToken);
            return Ok(hallDtos);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateHallDto createHallDto, CancellationToken cancellationToken)
        {
            var id = await _service.CreateAsync(createHallDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateHallDto updateHallDto,
            CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(id, updateHallDto, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> ChangeStatus(
            [FromRoute] int id,
            [FromBody] ChangeHallStatusDto changeStatusDto,
            CancellationToken cancellationToken)
        {
            await _service.ChangeStatusAsync(id, changeStatusDto, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id:int}/layout")]
        public async Task<ActionResult<HallLayoutDto>> GetLayout([FromRoute] int id, CancellationToken cancellationToken)
        {
            var layout = await _service.GetLayoutAsync(id, cancellationToken);
            return Ok(layout);
        }

        [HttpPut("{id:int}/layout")]
        public async Task<IActionResult> SetLayout(
            [FromRoute] int id,
            [FromBody] CreateSeatLayoutDto newSeatsDto,
            CancellationToken cancellationToken)
        {
            await _service.SetLayoutAsync(id, newSeatsDto, cancellationToken);
            return NoContent();
        }
    }
}
