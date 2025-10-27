using EventBus;
using MassTransit;
using Microsoft.OpenApi.Models;
using RabbitMQ;
using Trading.Application.Behaviors;
using Trading.Application.Interface;
using Trading.Infrastructure;
using Trading.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Trading.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<TradingDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("tradingdb"));
            });

            // MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Application.Commands.CreateOrderCommand).Assembly);
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            // FluentValidation
            services.AddValidatorsFromAssembly(typeof(Application.Commands.CreateOrderCommand).Assembly);

            // Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();

            // MassTransit
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqConnection = configuration.GetConnectionString("rabbitmq");
                    cfg.Host(new Uri(rabbitMqConnection!));
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IEventBus, EventBusRabbitMQ>();

            // Caching
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("redis");
            });

            // API
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MicroDragon Trading API",
                    Version = "v1",
                    Description = "Trading service for MicroDragon crypto platform"
                });
            });

            return services;
        }
    }
}
