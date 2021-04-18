using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using RabbitMqPostman.Common.Interfaces;

namespace RabbitMqPostman.Configuration.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly IApiLogger _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, IApiLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ILocalizerError localizer)
        {
            await _next.Invoke(context);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var ex = context.Features.Get<IExceptionHandlerFeature>();

            if (ex != null)
            {
                var err = localizer.BuildError(ex.Error);               

                await context.Response.WriteAsync(err).ConfigureAwait(false);

                _logger.LogError(ex.Error, err);
            }
        }
    }
}
