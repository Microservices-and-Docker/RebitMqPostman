using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;

namespace RabbitMqPostman.Configuration.Services
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

                //add sending jwtToken in header
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });
        }
    }
}
