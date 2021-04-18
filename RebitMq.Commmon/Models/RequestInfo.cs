using System;

namespace RabbitMqPostman.Common.Models
{
    public class RequestInfo
    {
        public RequestInfo()
        {
            UserInfo = new UserInfo();
        }

        public Guid CorrelationId { get; set; }
        public string AcceptLanguage { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class UserInfo
    {
        public string UserId { get; set; }
        public string AppId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreatedToken { get; set; }
    }
}
