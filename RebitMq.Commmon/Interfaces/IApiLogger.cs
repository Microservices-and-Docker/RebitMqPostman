using System;

namespace RabbitMqPostman.Common.Interfaces
{
    public interface IApiLogger
    {
        void LogInformationRequest(string requestBody, string url, string requestMethod);
        void LogInformationResponse(string responseBody, int statusCode, long procesingTime);
        void LogDebug(object message, string title = null);
        void LogError(Exception exception, string title = null);
    }
}
