using System;
using RabbitMqPostman.Models.v1;

namespace RabbitMqPostman.Interfaces
{
    public interface ILocalizer
    {
        string BuildError(Exception ex);
        ErrorResponse LocalizeTheError(Exception ex);
    }
}
