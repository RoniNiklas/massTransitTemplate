using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Shared.Messages;

namespace Server.Consumers;

public class WeatherChangeRequestAcceptedMessageConsumer : IConsumer<WeatherChangeRequestAcceptedMessage>
{
    private IHubContext<WeatherHub> _hubContext;

    public WeatherChangeRequestAcceptedMessageConsumer(IHubContext<WeatherHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<WeatherChangeRequestAcceptedMessage> context)
    {
        await _hubContext.Clients.All.SendAsync(
            nameof(IWeatherHubServerInvoked.WeatherChangeRequestValidatedAndPendingHandling)
        );
        await Task.Delay(5000);
        WeatherHubState.CurrentState = context.Message.Data;
        await _hubContext.Clients.All.SendAsync(
            nameof(IWeatherHubServerInvoked.WeatherHasChanged),
            WeatherHubState.CurrentState
        );
    }
}
