using Handlers.WeatherForecast;
using MassTransit;
using Microsoft.AspNetCore.ResponseCompression;
using Server.Consumers;
using Server.Hubs;
using Server.Infra.MediatR;

// SERVICES
var builder = WebApplication.CreateBuilder(args);

// LOGGER
builder.Logging.AddConsole();

// SWAGGER
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining(typeof(GetWeatherForecasts));

// MEDIATR
builder.Services.AddMediatR(typeof(GetWeatherForecastsQueryHandler));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:3000", "http://localhost:3001")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// SIGNALR
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddSignalR();

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
    x.AddConsumer<WeatherChangeRequestAcceptedMessageConsumer>();
    x.AddConsumer<WeatherChangeRequestDeniedMessageConsumer>();
    x.AddConsumer<WeatherChangeRequestConsumer>();
});

// APP
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

app.MapHub<WeatherHub>(IWeatherHubServerInvoked.Path);

app.Run();
