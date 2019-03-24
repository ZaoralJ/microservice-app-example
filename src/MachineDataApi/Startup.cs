namespace MachineDataApi
{
    using System;
    using Castle.Facilities.AspNetCore;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Interfaces;
    using Logging.NLog.Impl.Castle;
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
            Configuration cfg;

            if (Environment.GetEnvironmentVariable("RUNNING_IN_CONTAINER") != null)
            {
                cfg = new Configuration
                {
                    InfluxEndpoint = Environment.GetEnvironmentVariable("INFLUX_ENDPOINT"),
                    InfluxUserName = Environment.GetEnvironmentVariable("INFLUX_USERNAME"),
                    InfluxPassword = Environment.GetEnvironmentVariable("INFLUX_PASSWORD")
                };
            }
            else
            {
                var appSettingSection = Configuration.GetSection("Configuration");
                services.Configure<Configuration>(appSettingSection);

                cfg = appSettingSection.Get<Configuration>();
            }

            var container = new WindsorContainer();
            container.AddFacility<AspNetCoreFacility>(f => f.CrossWiresInto(services));
            container.AddFacility<TypedFactoryFacility>();

            container.Install(new LogInstaller());

            container.Register(
                Component.For<IReadMachineDataRepository>()
                         .ImplementedBy<Data.Influx.MachineDataRepository>()
                         .DependsOn(new
                         {
                             endpoint = cfg.InfluxEndpoint,
                             userName = cfg.InfluxUserName,
                             password = cfg.InfluxPassword
                         }));

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
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
