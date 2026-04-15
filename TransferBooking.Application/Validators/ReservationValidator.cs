using TransferBooking.Application.DTOs;

namespace TransferBooking.Application.Validators;

public static class ReservationValidator
{
	// Grupos de ubicaciones que pertenecen a la misma ciudad
	private static readonly List<HashSet<string>> _locationGroups = new()
	{
		new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"bogotá", "bogota", "aeropuerto el dorado", "el dorado",
			"usaquén", "chapinero", "suba", "engativa" , "kennedy", "bosa"
		},
		new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"medellín", "medellin", "aeropuerto olaya herrera", "olaya herrera",
			"el poblado", "laureles", "envigado", "bello", "itagui"
		},
		new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"cali", "aeropuerto alfonso bonilla aragón", "alfonso bonilla",
			"granada", "san antonio", "palmira"
		}
	};

	public static List<string> Validate(CreateReservationRequest request)
	{
		var errors = new List<string>();

		if (string.IsNullOrWhiteSpace(request.CustomerName))
			errors.Add("El nombre del cliente es obligatorio.");

		if (string.IsNullOrWhiteSpace(request.Origin))
			errors.Add("El origen es obligatorio.");

		if (string.IsNullOrWhiteSpace(request.Destination))
			errors.Add("El destino es obligatorio.");

		if (string.IsNullOrWhiteSpace(request.ServiceType))
			errors.Add("El tipo de servicio es obligatorio.");

		if (!string.IsNullOrWhiteSpace(request.Origin) && !string.IsNullOrWhiteSpace(request.Destination))
		{
			// Validación exacta
			if (request.Origin.Equals(request.Destination, StringComparison.OrdinalIgnoreCase))
			{
				errors.Add("El origen y destino no pueden ser iguales.");
			}
			else
			{
				// Validación por grupo de ciudad
				var sameCity = _locationGroups.Any(group =>
					group.Contains(request.Origin.Trim()) &&
					group.Contains(request.Destination.Trim()));

				if (sameCity)
					errors.Add("El origen y destino pertenecen a la misma ciudad. El traslado debe ser entre ciudades o hacia/desde un punto fuera de la ciudad de origen.");
			}
		}

		if (request.Passengers < 1 || request.Passengers > 6)
			errors.Add("El número de pasajeros debe estar entre 1 y 6.");

		if (request.Date == default)
			errors.Add("La fecha es obligatoria.");
		else if (request.Date < DateTime.Now)
			errors.Add("La fecha no puede estar en el pasado.");

		var validTypes = new[] { "standard", "premium" };
		if (!validTypes.Contains(request.ServiceType?.ToLower()))
			errors.Add("El tipo de servicio debe ser 'standard' o 'premium'.");

		return errors;
	}
}