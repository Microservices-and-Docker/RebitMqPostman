using System;

namespace RabbitMqPostman.Common.Models
{
    public class RabbitMqPostmanException : Exception
    {
        public RabbitMqPostmanException() { }

        public RabbitMqPostmanException(ErrorCodes errorCode)
        {
            ErrorCode = errorCode;
        }

        public RabbitMqPostmanException(ErrorCodes errorCode, string reason, string message = null)
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