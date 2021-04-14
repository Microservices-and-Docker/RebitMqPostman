﻿using Microsoft.AspNetCore.Builder;

namespace RebitMqPostman.Configuration.Middlewares
{
    public static class SwaggerExtensions
    {
        public static IApplicationBuilder AddSwaggerDefaultRoute(this IApplicationBuilder app, string fullAppName)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("/swagger.css");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", fullAppName);
                c.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}