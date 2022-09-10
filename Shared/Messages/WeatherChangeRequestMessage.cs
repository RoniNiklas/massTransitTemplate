namespace Shared.Messages;
public record WeatherChangeRequestMessage(Guid Guid, GetSingleWeatherForecast Request);