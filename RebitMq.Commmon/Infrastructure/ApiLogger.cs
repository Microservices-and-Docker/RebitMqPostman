using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMqPostman.Common.Interfaces;
using RabbitMqPostman.Common.Models;

namespace RabbitMqPostman.Common.Infrastructure
{
    public class ApiLogger : IApiLogger
    {
        private readonly RequestInfo _requestInfo;
        private readonly ILogger<ApiLogger> _logger;

        public ApiLogger(ILogger<ApiLogger> logger, IOptions<RequestInfo> requestInfo)
        {
            _logger = logger;
            _requestInfo = requestInfo.Value;
        }
        public void LogInformationRequest(string requestBody, string url, string requestMethod)
        {
            var request = new StringBuilder();
            request.Append($"\n  Request: [{_requestInfo.CorrelationId}]");
            request.Append($"\n  [{requestMethod}] {url}");

            if (_requestInfo.UserInfo.PhoneNumber != null)
                request.Append($"\n  Phone: {_requestInfo.UserInfo.PhoneNumber}");
            if (_requestInfo.UserInfo.AppId != null)
                request.Append($"\n  AppId: {_requestInfo.UserInfo.AppId}");
            if (_requestInfo.UserInfo.UserId != null)
                request.Append($"\n  UserId: {_requestInfo.UserInfo.UserId}");

            request.Append($"\n  RequestBody: {requestBody.Replace("\n", "")}");
            request.Append($"{Environment.NewLine}");

            _logger.LogInformation(request.ToString());
        }

        public void LogInformationResponse(string responseBody, int statusCode, long procesingTime)
        {
            var request = new StringBuilder();
            request.Append($"\n  Response: [{_requestInfo.CorrelationId}]");
            request.Append($"\n  StatusCode: {statusCode}");
            request.Append($"\n  ProcesingTime: {TimeSpan.FromMilliseconds(procesingTime).TotalSeconds} sec");
            request.Append($"\n  ResponseBody: {responseBody}");
            request.Append($"{Environment.NewLine}");

            _logger.LogInformation(request.ToString());
        }

        public void LogDebug(object message, string title = null)
        {
            var request = new StringBuilder();
            request.Append($"  [{_requestInfo.CorrelationId}]");

            if (title != null)
                request.Append($"\n  Tittle: {title}");

            request.Append($"\n  Message: {JsonConvert.SerializeObject(message).Replace("\n", "")}");
            request.Append($"{Environment.NewLine}");

            _logger.LogDebug(request.ToString());
        }

        public void LogError(Exception exception, string title = null)
        {
            var request = new StringBuilder();
            request.Append($"  [{_requestInfo.CorrelationId}]");

            if (title != null)
                request.Append($"\n  Tittle: {title}");

            if (exception is ApiException)
                request.Append($"\n  Message: {exception as ApiException}");
            else
                request.Append($"\n  Message: {exception}");

            request.Append($"{Environment.NewLine}");

            _logger.LogError(request.ToString());
        }
    }
}
