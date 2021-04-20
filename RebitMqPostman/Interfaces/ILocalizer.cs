using System;
using RabbitMqPostman.Common.Models;

namespace RabbitMqPostman.Interfaces
{
    public interface ILocalizer
    {
        string BuildError(Exception ex);
        ApiException LocalizeTheError(Exception ex);
    }
}
