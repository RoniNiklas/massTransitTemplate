namespace Shared.Hubs;

public interface IWeatherHubServerInvoked
{
    public const string Path = "/weather";
    Task WeatherHasChanged(RequestResult<WeatherForecastViewModel> weather);
    Task WeatherChangeRequestAcceptedForHandling();
    Task WeatherChangeRequestValidatedAndPendingHandling();
}

public interface IWeatherHubClientInvoked
{
    Task SyncState();
    Task UserRequestsWeatherChange(int id);
}
