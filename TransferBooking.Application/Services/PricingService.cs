using TransferBooking.Application.DTOs;

namespace TransferBooking.Application.Services;

public static class PricingService
{
	public static decimal Calculate(CreateReservationRequest request)
	{
		// Zona horaria de Colombia (UTC-5)
		var colombiaZone = TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");
		var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, colombiaZone).Date;

		bool isPremium = request.ServiceType.ToLower() == "premium";
		decimal basePrice = isPremium ? 80000m : 50000m;

		// +10.000 por pasajero
		decimal price = basePrice + (request.Passengers * 10000m);

		// +20% si es el mismo día
		if (request.Date.Date == today)
			price *= 1.20m;

		// +15% si hay más de 4 pasajeros
		if (request.Passengers > 4)
			price *= 1.15m;

		// +10% adicional si es premium y más de 3 pasajeros
		if (isPremium && request.Passengers > 3)
			price *= 1.10m;

		// -5% si la reserva es con 2+ días de anticipación
		if ((request.Date.Date - today).TotalDays >= 2)
			price *= 0.95m;

		return Math.Round(price, 2);
	}
}