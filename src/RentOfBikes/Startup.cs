using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Domain.Entities.CQRS;
using Autofac;
using Infrastructure.Command;
using Infrastructure.Query;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Domain.Repositories;
using Domain.Services;

namespace RentOfBikes
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            var container = new ContainerBuilder();

            container
                .RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .SingleInstance();

            container
                .RegisterType<BikeNameVerifier>()
                .As<IBikeNameVerifier>();

            container
                .RegisterType<BikeService>()
                .As<IBikeService>();

            container
                .RegisterType<DepositCalculator>()
                .As<IDepositCalculator>();

            container
                .RegisterType<RentService>()
                .As<IRentService>();

            container
                .RegisterType<DepositService>()
                .As<IDepositService>();

            container
                .RegisterAssemblyTypes(typeof(AddNewBikeCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICommand<>));

            container
                .RegisterType<CommandBuilder>()
                .As<ICommandBuilder>();

            container
                .RegisterTypedFactory<ICommandFactory>();

            container
              .RegisterAssemblyTypes(typeof(GetAllBikes).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(IQuery<,>));

            container
                .RegisterGeneric(typeof(QueryFor<>))
                .As(typeof(IQueryFor<>));

            container
                .RegisterTypedFactory<IQueryBuilder>();

            container
                .RegisterTypedFactory<IQueryFactory>();

            container.Populate(services);

            this.ApplicationContainer = container.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=RentPoints}/{action=RentPoints}/{id?}");
            });
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
