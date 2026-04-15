using TransferBooking.Application.DTOs;
using TransferBooking.Application.Validators;
using TransferBooking.Domain.Entities;
using TransferBooking.Domain.Enums;
using TransferBooking.Domain.Interfaces;

namespace TransferBooking.Application.Services;

public class ReservationService
{
	private readonly IReservationRepository _repository;

	public ReservationService(IReservationRepository repository)
	{
		_repository = repository;
	}

	public async Task<IEnumerable<ReservationResponse>> GetAllAsync()
	{
		var reservations = await _repository.GetAllAsync();
		return reservations.Select(MapToResponse);
	}

	public async Task<ReservationResponse?> GetByIdAsync(Guid id)
	{
		var reservation = await _repository.GetByIdAsync(id);
		return reservation is null ? null : MapToResponse(reservation);
	}

	public async Task<(ReservationResponse? result, List<string> errors)> CreateAsync(CreateReservationRequest request)
	{
		var errors = ReservationValidator.Validate(request);
		if (errors.Count > 0) return (null, errors);

		bool duplicate = await _repository.ExistsAsync(
			request.CustomerName, request.Origin, request.Destination,
			request.Date, request.ServiceType);

		if (duplicate)
		{
			errors.Add("Ya existe una reserva idéntica.");
			return (null, errors);
		}

		var reservation = new Reservation
		{
			CustomerName = request.CustomerName,
			Origin = request.Origin,
			Destination = request.Destination,
			Date = request.Date,
			Passengers = request.Passengers,
			ServiceType = Enum.Parse<ServiceType>(request.ServiceType, ignoreCase: true),
			Price = PricingService.Calculate(request)
		};

		var created = await _repository.CreateAsync(reservation);
		return (MapToResponse(created), errors);
	}

	public async Task<(ReservationResponse? result, string? error)> ConfirmAsync(Guid id)
	{
		var reservation = await _repository.GetByIdAsync(id);
		if (reservation is null) return (null, "Reserva no encontrada.");
		if (reservation.Status == ReservationStatus.Cancelled) return (null, "No se puede confirmar una reserva cancelada.");
		if (reservation.Status == ReservationStatus.Confirmed) return (null, "La reserva ya está confirmada.");

		reservation.Status = ReservationStatus.Confirmed;
		var updated = await _repository.UpdateAsync(reservation);
		return (MapToResponse(updated), null);
	}

	public async Task<(ReservationResponse? result, string? error)> CancelAsync(Guid id)
	{
		var reservation = await _repository.GetByIdAsync(id);
		if (reservation is null) return (null, "Reserva no encontrada.");
		if (reservation.Status == ReservationStatus.Cancelled) return (null, "La reserva ya está cancelada.");

		reservation.Status = ReservationStatus.Cancelled;
		var updated = await _repository.UpdateAsync(reservation);
		return (MapToResponse(updated), null);
	}

	private static ReservationResponse MapToResponse(Reservation r) => new()
	{
		Id = r.Id,
		CustomerName = r.CustomerName,
		Origin = r.Origin,
		Destination = r.Destination,
		Date = r.Date,
		Passengers = r.Passengers,
		ServiceType = r.ServiceType.ToString(),
		Status = r.Status.ToString(),
		Price = r.Price,
		CreatedAt = r.CreatedAt
	};
}