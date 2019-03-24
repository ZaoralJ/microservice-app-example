namespace MachineSqlDataService
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.MsDependencyInjection;
    using Data.Sql;
    using EventBus;
    using EventBus.RabbitMQ;
    using EventLogger.NLog;
    using Interfaces;
    using Logging.NLog.Impl.Castle;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    internal class Program
    {
        internal static async Task Main()
        {
            var host = new HostBuilder()
                       .ConfigureAppConfiguration(
                           (context, builder) =>
                               {
                                   builder.AddJsonFile("appsettings.json");
                               })
                       .ConfigureServices(ConfigureServices)
                       .Build();

            await host.RunAsync();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            Configuration cfg;

            if (Environment.GetEnvironmentVariable("RUNNING_IN_CONTAINER") != null)
            {
                cfg = new Configuration
                {
                    RabbitMqBrokerName = Environment.GetEnvironmentVariable("RABBITMQ_BROKER_NAME"),
                    RabbitMqQueueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE_NAME"),
                    RabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
                    RabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER"),
                    RabbitMqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"),
                    RabbitMqPrefetchCount = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PREFETCH_COUNT"), CultureInfo.InvariantCulture),
                    SqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
                };
            }
            else
            {
                cfg = context.Configuration.GetSection("Configuration").Get<Configuration>();
            }

            var container = new WindsorContainer();

            container.AddFacility<TypedFactoryFacility>();

            container.Install(new LogInstaller());

            container.Register(
                Component.For<IIntegrationEventHandlerFactory>().AsFactory(new IntegrationEventHandlerComponentSelector()),

                Component.For<IEventBusSubscriptionsManager>()
                         .ImplementedBy<InMemoryEventBusSubscriptionsManager>(),

                Component.For<IEventLogger>()
                         .ImplementedBy<EventLogger>(),

                Component.For<IEventBus>()
                         .ImplementedBy<EventBusRabbitMQ>()
                         .DependsOn(new
                         {
                             brokerName = cfg.RabbitMqBrokerName,
                             queueName = cfg.RabbitMqQueueName,
                             prefetchCount = cfg.RabbitMqPrefetchCount
                         }),

                Component.For<IRabbitMQPersistentConnection>()
                         .ImplementedBy<DefaultRabbitMQPersistentConnection>()
                         .DependsOn(new
                         {
                             hostName = cfg.RabbitMqHost,
                             userName = cfg.RabbitMqUser,
                             password = cfg.RabbitMqPassword
                         }),

                Component.For<MachineDataIntegrationEventHandler>(),

                Component.For<IWriteMachineDataRepository>()
                         .ImplementedBy<MachineDataRepository>()
                         .DependsOn(new { connectionString = cfg.SqlConnectionString }),

                Component.For<Service>());

            WindsorRegistrationHelper.CreateServiceProvider(container, services);

            services.AddSingleton<IHostedService>(container.Resolve<Service>());
        }
    }
}
