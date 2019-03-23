namespace MachineService
{
    using Castle.Facilities.AspNetCore;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using EventBus;
    using EventBus.RabbitMQ;
    using Logging.NLog.Impl.Castle;
    using MachineService.Core;
    using MachineService.Factories;
    using MachineService.Managers;
    using MachineService.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var container = new WindsorContainer();
            container.AddFacility<AspNetCoreFacility>(f => f.CrossWiresInto(services));
            container.AddFacility<TypedFactoryFacility>();

            container.Install(new LogInstaller());

            container.Register(
                Component.For<IMachineFactory>().AsFactory(),
                
                Component.For<IIntegrationEventHandlerFactory>().AsFactory(new DefaultTypedFactoryComponentSelector()),

                Component.For<IEventBusSubscriptionsManager>()
                         .ImplementedBy<InMemoryEventBusSubscriptionsManager>(),

                Component.For<IEventBus>()
                         .ImplementedBy<EventBusRabbitMQ>()
                         .DependsOn(new { brokerName = "eventbus" }),

                Component.For<IRabbitMQPersistentConnection>()
                         .ImplementedBy<DefaultRabbitMQPersistentConnection>()
                         .DependsOn(new
                         {
                             hostName = "localhost",
                             userName = "guest",
                             password = "guest"
                         }),

                Component.For<IMachine>()
                         .ImplementedBy<Machine>()
                         .LifestyleTransient(),

                Component.For<IMachineManager>()
                         .ImplementedBy<MachineManager>());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddWindsor(container, opts => opts.UseEntryAssembly(typeof(IRef).Assembly));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
