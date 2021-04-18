using System;

namespace RabbitMqPostman.Common.Models
{
    public class ApiException : Exception
    {
        public ApiException() { }

        public ApiException(ErrorCodes errorCode)
        {
            ErrorCode = errorCode;
        }

        public ApiException(ErrorCodes errorCode, string reason, string message = null)
                                 : base(message)
        {
            Reason = reason;
            ErrorCode = errorCode;
        }

        public ErrorCodes ErrorCode { get; }

        /// <summary> Причина </summary>
        public string Reason { get; set; }
    }
}