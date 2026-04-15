using TransferBooking.Domain.Enums;

namespace TransferBooking.Domain.Entities;

public class Reservation
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string CustomerName { get; set; } = string.Empty;
	public string Origin { get; set; } = string.Empty;
	public string Destination { get; set; } = string.Empty;
	public DateTime Date { get; set; }
	public int Passengers { get; set; }
	public ServiceType ServiceType { get; set; }
	public ReservationStatus Status { get; set; } = ReservationStatus.Created;
	public decimal Price { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}