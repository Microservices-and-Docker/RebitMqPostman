using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMqPostman.Common.Models;

namespace RabbitMqPostman.Common.Infrastructure
{
    public static class LogExtensions
    {
        public static void LogInformationRequest(this ILogger logger, RequestInfo requestInfo, string requestBody, string url, string requestMethod)
        {
            var request = new StringBuilder();
            request.Append($"\n  Request: [{requestInfo.CorrelationId}]");
            request.Append($"\n  [{requestMethod}] {url}");

            if (requestInfo.UserInfo.PhoneNumber != null)
                request.Append($"\n  Phone: {requestInfo.UserInfo.PhoneNumber}");
            if (requestInfo.UserInfo.AppId != null)
                request.Append($"\n  AppId: {requestInfo.UserInfo.AppId}");
            if (requestInfo.UserInfo.UserId != null)
                request.Append($"\n  UserId: {requestInfo.UserInfo.UserId}");

            request.Append($"\n  RequestBody: {requestBody.Replace("\n", "")}");
            request.Append($"{Environment.NewLine}");

            logger.LogInformation(request.ToString());
        }

        public static void LogInformationResponse(this ILogger logger, Guid correlationId, string responseBody, int statusCode, long procesingTime)
        {
            var request = new StringBuilder();
            request.Append($"\n  Response: [{correlationId}]");
            request.Append($"\n  StatusCode: {statusCode}");
            request.Append($"\n  ProcesingTime: {TimeSpan.FromMilliseconds(procesingTime).TotalSeconds} sec");
            request.Append($"\n  ResponseBody: {responseBody}");
            request.Append($"{Environment.NewLine}");

            logger.LogInformation(request.ToString());
        }

        public static void LogDebug(this ILogger logger, Guid correlationId, object message, string title = null)
        {
            var request = new StringBuilder();
            request.Append($"  [{correlationId}]");

            if (title != null)
                request.Append($"\n  Tittle: {title}");

            request.Append($"\n  Message: {JsonConvert.SerializeObject(message).Replace("\n", "")}");
            request.Append($"{Environment.NewLine}");

            logger.LogDebug(request.ToString());
        }

        public static void LogError(this ILogger logger, Guid correlationId, Exception exception, string title = null)
        {
            var request = new StringBuilder();
            request.Append($"  [{correlationId}]");

            if (title != null)
                request.Append($"\n  Tittle: {title}");

            request.Append($"\n  Message: {exception}");
            request.Append($"{Environment.NewLine}");

            logger.LogError(request.ToString());
        }
    }
}
