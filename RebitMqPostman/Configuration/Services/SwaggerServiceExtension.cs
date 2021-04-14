using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RebitMqPostman.Configuration.Services
{
    public static class SwaggerServiceExtension
    {
        public static void AddSwaggerService(this IServiceCollection services, string description)
        {
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
