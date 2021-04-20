namespace RabbitMqPostman.Models.v1
{
    public class ErrorResponse
    {
        public ErrorResponse() { }
        public ErrorResponse(string message)
        {
            Message = message;
        }

        public ErrorResponse(string message, string reason)
        {
            Message = message;
            Reason = reason;
        }

        public string Message { get; set; }
        public string Reason { get; set; }
    }
}
