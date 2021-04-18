using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RabbitMqPostman.Common.Models;
using RabbitMqPostman.Infrastructure;

namespace RabbitMqPostman.Configuration.Middlewares
{
    public class CorrelationMiddleware
    {
        private RequestInfo _requestInfo;
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next, IOptions<RequestInfo> requestInfo)
        {
            _next = next;
            _requestInfo = requestInfo.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            AddCorrelationId(context);
            GetHeaderItems(context);

            await _next.Invoke(context);
        }

        private void AddCorrelationId(HttpContext context)
        {
            string correlationId = context.Request.Headers[ConstantsRabbitMqPostman.CorrelationIdKey];

            if (correlationId != null
             && Guid.TryParse(correlationId, out Guid correlationIdGuid))
                _requestInfo.CorrelationId = correlationIdGuid;
            else
                _requestInfo.CorrelationId = Guid.NewGuid();
        }

        private void GetHeaderItems(HttpContext context)
        {
            foreach (var item in context.Request.Headers)
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                    break;

                if (item.Key == ConstantsRabbitMqPostman.CorrelationLanguage)
                    _requestInfo.AcceptLanguage = item.Value;

                //Если есть другие ключи в хедере
                /*
               else if (item.Key == ConstantsRebitMqPostman.CorrelationPhoneKey)
                   _requestInfo.PhoneNumber = item.Value;
               else if (item.Key == ConstantsRebitMqPostman.CorrelationUserId)
                   _requestInfo.UserId = item.Value;
                */
            }
        }
    }
}
