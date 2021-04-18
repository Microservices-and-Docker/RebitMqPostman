namespace RabbitMqPostman.Common.Models
{
    public enum ErrorCodes
    {
        Undefined,

        JwtToken_IsEmpty,
        JwtToken_SignatureValidationFailed,
        JwtToken_TokenIsExpired,

        RabbitMq_ConectionError,

        Error_HandleCreateCustomer
    }
}
