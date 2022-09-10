using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Shared.Messages;

namespace Server.Consumers;

public class WeatherChangeRequestDeniedMessageConsumer : IConsumer<WeatherChangeRequestDeniedMessage>
{
    private IHubContext<WeatherHub> _hubContext;

    public WeatherChangeRequestDeniedMessageConsumer(IHubContext<WeatherHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<WeatherChangeRequestDeniedMessage> context)
    {
        WeatherHubState.CurrentState = new(context.Message.Error);
        await _hubContext.Clients.All.SendAsync(
            nameof(IWeatherHubServerInvoked.WeatherHasChanged),
            WeatherHubState.CurrentState
        );
    }
}
