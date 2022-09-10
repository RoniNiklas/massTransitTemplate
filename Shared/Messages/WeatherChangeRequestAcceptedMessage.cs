namespace Shared.Messages;
public record WeatherChangeRequestAcceptedMessage(Guid Guid, WeatherForecastViewModel Data);
