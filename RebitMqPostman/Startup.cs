using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using RabbitMqPostman.BLL;
using RabbitMqPostman.Configuration.Services;
using RabbitMqPostman.Configuration.Middlewares;
using RabbitMqPostman.Common.Models;
using RabbitMqPostman.Common.Interfaces;
using RabbitMqPostman.Common.Infrastructure;
using RabbitMqPostman.Resources.ErrorMessage;

namespace RabbitMqPostman
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appConfigurationStr = Configuration.GetSection("AppSettings");
            var appConfiguration = appConfigurationStr.Get<AppSettings>();

            services.Configure<AppSettings>(appConfigurationStr);

            services.AddControllers();
            services.AddVersioning();
            services.AddLocalizationService();
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSwaggerService("RebitMq Postman");
            services.AddNLogConfiguration(appConfiguration);
            services.AddResponseCaches(Configuration);

            services.ConfigureBLL(Configuration);

            services.AddScoped<RequestInfo>();
            services.AddScoped<ILocalizerError, LocalizerError<LocalizationErrorMessage>>();

            services.AddTransient<IApiLogger, ApiLogger>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.AddSwaggerDefaultRoute("RebitMqPostman API V1");
            app.UseRouting();

            //todo add validator          

            app.UseRequestLocalization();
            //app.UseMiddleware<JwtTokenMiddleware>();
            app.UseMiddleware<CorrelationMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseExceptionHandler(options => options.UseMiddleware<ExceptionMiddleware>());

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseResponseCaching();
        }
    }
}
