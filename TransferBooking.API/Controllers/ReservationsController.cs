using Microsoft.AspNetCore.Mvc;
using TransferBooking.Application.DTOs;
using TransferBooking.Application.Services;

namespace TransferBooking.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationsController : ControllerBase
{
	private readonly ReservationService _service;

	public ReservationsController(ReservationService service)
	{
		_service = service;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var result = await _service.GetAllAsync();
		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var result = await _service.GetByIdAsync(id);
		if (result is null)
			return NotFound(new { message = "Reserva no encontrada." });
		return Ok(result);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
	{
		var (result, errors) = await _service.CreateAsync(request);
		if (errors.Count > 0)
			return BadRequest(new { errors });
		return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
	}

	[HttpPatch("{id:guid}/confirm")]
	public async Task<IActionResult> Confirm(Guid id)
	{
		var (result, error) = await _service.ConfirmAsync(id);
		if (error is not null)
			return BadRequest(new { message = error });
		return Ok(result);
	}

	[HttpPatch("{id:guid}/cancel")]
	public async Task<IActionResult> Cancel(Guid id)
	{
		var (result, error) = await _service.CancelAsync(id);
		if (error is not null)
			return BadRequest(new { message = error });
		return Ok(result);
	}
}