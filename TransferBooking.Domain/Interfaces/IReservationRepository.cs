using TransferBooking.Domain.Entities;

namespace TransferBooking.Domain.Interfaces;

public interface IReservationRepository
{
	Task<IEnumerable<Reservation>> GetAllAsync();
	Task<Reservation?> GetByIdAsync(Guid id);
	Task<bool> ExistsAsync(string customerName, string origin, string destination, DateTime date, string serviceType);
	Task<Reservation> CreateAsync(Reservation reservation);
	Task<Reservation> UpdateAsync(Reservation reservation);
}