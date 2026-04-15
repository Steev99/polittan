using TransferBooking.Domain.Entities;
using TransferBooking.Domain.Interfaces;

namespace TransferBooking.Infrastructure.Repositories;

public class InMemoryReservationRepository : IReservationRepository
{
	private readonly List<Reservation> _reservations = new();

	public Task<IEnumerable<Reservation>> GetAllAsync()
	{
		return Task.FromResult<IEnumerable<Reservation>>(_reservations);
	}

	public Task<Reservation?> GetByIdAsync(Guid id)
	{
		var reservation = _reservations.FirstOrDefault(r => r.Id == id);
		return Task.FromResult(reservation);
	}

	public Task<bool> ExistsAsync(string customerName, string origin, string destination, DateTime date, string serviceType)
	{
		var exists = _reservations.Any(r =>
			r.CustomerName.Equals(customerName, StringComparison.OrdinalIgnoreCase) &&
			r.Origin.Equals(origin, StringComparison.OrdinalIgnoreCase) &&
			r.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase) &&
			r.Date == date &&
			r.ServiceType.ToString().Equals(serviceType, StringComparison.OrdinalIgnoreCase));

		return Task.FromResult(exists);
	}

	public Task<Reservation> CreateAsync(Reservation reservation)
	{
		_reservations.Add(reservation);
		return Task.FromResult(reservation);
	}

	public Task<Reservation> UpdateAsync(Reservation reservation)
	{
		var index = _reservations.FindIndex(r => r.Id == reservation.Id);
		if (index >= 0)
			_reservations[index] = reservation;
		return Task.FromResult(reservation);
	}
}