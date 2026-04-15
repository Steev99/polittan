#                                                       POLITTAN

# TransferBooking API
API REST para una plataforma de reservas de traslados, desarrollada como prueba técnica para el cargo de Desarrollador Backend .NET Senior.

# Tecnologías utilizadas
- .NET 10
- ASP.NET Core Web API
- Arquitectura en capas (Domain, Application, Infrastructure, API)
- Swagger / OpenAPI para documentación
- Almacenamiento en memoria (sin base de datos)

# Estructura del proyecto

<img width="898" height="637" alt="image" src="https://github.com/user-attachments/assets/ab0d9416-f2ea-4d0a-9cbe-8f4575dbf3b5" />


# Instrucciones para ejecutar

## Requisitos previos
- .NET 10 SDK
- Visual Studio 2022 o superior (recomendado)

## Pasos
- Clona el repositorio: git clone https://github.com/tu-usuario/transfer-booking-api.git
cd transfer-booking-api
- Ejecuta el proyecto: cd src/TransferBooking.API
dotnet run
- Abre el navegador en: https://localhost:7123/swagger
  
# Endpoints disponibles

## Método             Ruta                                 Descripción
   POST               /Reservations                        Crear una nueva reserva
   GET                /Reservations                        Obtener todas las reservas
   GET                /Reservations/{id}                   Obtener una reserva por ID
   PATCH              /Reservations/{id}/confirm           Confirmar una reserva
   PATCH              /Reservations/{id}/cancel            Cancelar una reserva


# Ejemplo de uso
## Crear reserva
POST /Reservations
{
  "customerName": "Juan Pérez",
  "origin": "Bogotá",
  "destination": "Medellin",
  "date": "2026-04-20T10:00:00",
  "passengers": 3,
  "serviceType": "standard"
}

## Respuesta exitosa (201)
{
  "id": "a05fb7b2-e4ef-4ae6-bec0-e4920197fd80",
  "customerName": "Juan Pérez",
  "origin": "Bogotá",
  "destination": "Medellin",
  "date": "2026-04-20T10:00:00",
  "passengers": 3,
  "serviceType": "Standard",
  "status": "Created",
  "price": 76000,
  "createdAt": "2026-04-15T00:46:12Z"
}

# Reglas de precio
## Regla                                                Valor
   Precio base Standard                                 50.000 COP
   Precio base Premium                                  80.000 COP  
   Por pasajero                                         +10.000 COP
   Reserva el mismo día                                 +20%
   Más de 4 pasajeros                                   +15%   
   Premium con más de 3 pasajeros                       +10% 
   adicionalReserva con 2+ días de anticipación         -5%

# Ejemplo de cálculo
- Servicio: Standard: base 50.000
- 3 pasajeros: +30.000 = 80.000
- 6 días de anticipación: -5% = 76.000 COP

# Validaciones implementadas
- Todos los campos son obligatorios
- Pasajeros entre 1 y 6
- Fecha no puede estar en el pasado
- Origen y destino no pueden ser iguales
- Origen y destino no pueden pertenecer a la misma ciudad (validación por grupos de ubicaciones conocidas)
- No se permiten reservas duplicadas (mismo cliente, origen, destino, fecha y tipo de servicio)

# Estados de una reserva
- Created = Confirmed
- Created = Cancelled
- Confirmed = Cancelled

- Una reserva cancelada no puede confirmarse
- Una reserva confirmada no puede confirmarse de nuevo
- Una reserva cancelada no puede cancelarse de nuevo

# Supuestos de diseño
- Almacenamiento en memoria: Los datos se pierden al reiniciar el servidor. Se usó Singleton para el repositorio para garantizar que los datos persistan durante toda la sesión de ejecución.
- Validación de ciudad: La validación de origen ≠ destino compara los strings exactos. Adicionalmente, se implementó una validación por grupos de ubicaciones conocidas (ej: "Bogotá" y "Aeropuerto El Dorado" pertenecen al mismo grupo). Esta lista puede extenderse fácilmente.
- Cálculo de precio: Los porcentajes se aplican de forma acumulativa sobre el precio resultante del paso anterior, no todos sobre el precio base.
- Fechas en UTC: CreatedAt se almacena en UTC. La fecha de la reserva se toma tal como la envía el cliente.
- ServiceType como string en el request: Se recibe como string para facilitar el consumo desde el cliente y se parsea internamente al enum.

# Mejoras futuras
- Base de datos real: Integrar Entity Framework Core con SQL Server o PostgreSQL para persistencia real.
- Autenticación y autorización: Implementar JWT para proteger los endpoints.
- Paginación: Agregar paginación al endpoint GET /Reservations para manejar grandes volúmenes.
- Geocodificación: Integrar una API como Google Maps para validar ciudad de origen y destino por coordenadas, en lugar de una lista estática.
- Pruebas unitarias: Agregar cobertura de tests para PricingService, ReservationValidator y ReservationService.
- Logs estructurados: Implementar Serilog para trazabilidad de operaciones.
- Manejo global de errores: Agregar un middleware de excepciones para respuestas de error consistentes.
- Cancelación con motivo: Permitir enviar un motivo al cancelar una reserva.
- Notificaciones: Enviar email o SMS al confirmar/cancelar una reserva.
