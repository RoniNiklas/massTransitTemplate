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
        var valRes = context.Message.Request.Validate();

        await Task.Delay(3000);

        if (valRes.IsValid)
        {
            var weather = new WeatherForecastViewModel
                (
                    DateTime.Now.AddDays(context.Message.Request.Id),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                );
            await context.Publish(new WeatherChangeRequestAcceptedMessage(context.Message.Guid, weather));
            return;
        }

        await context.Publish(
            new WeatherChangeRequestDeniedMessage(
                context.Message.Guid,
                new(valRes.Errors)
        ));
    }
}
