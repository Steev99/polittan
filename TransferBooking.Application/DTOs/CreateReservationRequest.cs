namespace TransferBooking.Application.DTOs;

public class CreateReservationRequest
{
	public string CustomerName { get; set; } = string.Empty;
	public string Origin { get; set; } = string.Empty;
	public string Destination { get; set; } = string.Empty;
	public DateTime Date { get; set; }
	public int Passengers { get; set; }
	public string ServiceType { get; set; } = string.Empty;
}