namespace Shared.Messages;
public record WeatherChangeRequestDeniedMessage(Guid Guid, ValidationError Error);