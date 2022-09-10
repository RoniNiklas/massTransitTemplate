using MassTransit;
using Shared.Messages;

namespace Server.Consumers;


public class WeatherChangeRequestConsumer : IConsumer<WeatherChangeRequestMessage>
{
    public WeatherChangeRequestConsumer()
    {
    }

    private static readonly string[] _summaries = new[]
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };

    public async Task Consume(ConsumeContext<WeatherChangeRequestMessage> context)
    {
        var msg = context.Message;
        var valRes = msg.Request.Validate();

        await Task.Delay(3000);

        if (valRes.IsValid)
        {
            var weather = new WeatherForecastViewModel
                (
                    DateTime.Now.AddDays(msg.Request.Id),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                );
            await context.Publish(
                new WeatherChangeRequestAcceptedMessage(
                        msg.Guid,
                        msg.GroupId,
                        weather));
            return;
        }

        await context.Publish(
            new WeatherChangeRequestDeniedMessage(
                msg.Guid,
                msg.GroupId,
                new(valRes.Errors)
        ));
    }
}
