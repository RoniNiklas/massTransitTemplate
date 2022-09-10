namespace Shared.Messages;
public record WeatherChangeRequestDeniedMessage(Guid Guid, string GroupId, ValidationError Error);