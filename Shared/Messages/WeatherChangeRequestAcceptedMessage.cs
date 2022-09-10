namespace Shared.Messages;
public record WeatherChangeRequestAcceptedMessage(Guid Guid, string GroupId, WeatherForecastViewModel Data);
