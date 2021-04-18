using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using RabbitMqPostman.Common.Interfaces;

namespace RabbitMqPostman.Configuration.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly IApiLogger _logger;
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IApiLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.GetDisplayUrl()))
            {
                var sw = Stopwatch.StartNew();

                await LogRequest(context);
                await LogResponse(context, sw);
            }
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var request = context.Request;

            string requestMethod = request.Method;
            string url = context.Request.GetDisplayUrl();
            string requestBody = await new StreamReader(request.Body, Encoding.Default).ReadToEndAsync();

            request.Body.Position = 0;

            _logger.LogInformationRequest(requestBody, url, requestMethod);
        }

        private async Task LogResponse(HttpContext context, Stopwatch sw)
        {
            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                await _next.Invoke(context);

                buffer.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(buffer);
                using (var bufferReader = new StreamReader(buffer))
                {
                    string response = await bufferReader.ReadToEndAsync();

                    buffer.Seek(0, SeekOrigin.Begin);
                    await buffer.CopyToAsync(stream);
                    context.Response.Body = stream;

                    sw.Stop();

                    _logger.LogInformationResponse(response, context.Response.StatusCode, sw.ElapsedMilliseconds);
                }
            }
        }
    }
}
