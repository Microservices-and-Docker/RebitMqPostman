using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using RebitMqPostman.BLL;
using RebitMqPostman.Configuration.Services;
using RebitMqPostman.Configuration.Middlewares;
using RebitMqPostman.Common.Models;

namespace RebitMqPostman
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
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSwaggerService("RebitMq Postman");
            services.AddNLogConfiguration(appConfiguration);

            services.ConfigureBLL(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.AddSwaggerDefaultRoute("RebitMqPostman API V1");
            app.UseRouting();

            app.UseAuthorization();
            //todo add request/response logger
            //todo добавить обработку ошибок
            //todo add validator

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
