using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.Messages;

namespace Server.Hubs;

public class WeatherHub : Hub<IWeatherHubServerInvoked>, IWeatherHubClientInvoked
{
    private readonly IPublishEndpoint _publishEndpoint;
    public WeatherHub(IPublishEndpoint publishEndpoint) : base()
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task SyncState()
    {
        await Clients.Caller.WeatherHasChanged(WeatherHubState.CurrentState);
    }

    public async Task UserRequestsWeatherChange(int id)
    {
        await _publishEndpoint.Publish(
            new WeatherChangeRequestMessage(
                Guid.NewGuid(),
                new GetSingleWeatherForecast(id)));
        await Clients.Caller.WeatherChangeRequestAcceptedForHandling();
    }
}

internal static class WeatherHubState
{
    internal static RequestResult<WeatherForecastViewModel>? CurrentState { get; set; }
}