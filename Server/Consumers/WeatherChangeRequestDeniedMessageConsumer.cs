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
        var msg = context.Message;
        await _hubContext.Clients.Group(msg.GroupId).SendAsync(
            nameof(IWeatherHubServerInvoked.WeatherHasChanged),
            new RequestResult(msg.Error)
        );
    }
}
