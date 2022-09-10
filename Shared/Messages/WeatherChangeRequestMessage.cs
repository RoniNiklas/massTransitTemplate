namespace Shared.Messages;
public record WeatherChangeRequestMessage(Guid Guid, string GroupId, GetSingleWeatherForecast Request);