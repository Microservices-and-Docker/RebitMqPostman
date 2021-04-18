using System;
using RabbitMqPostman.Common.Models;

namespace RabbitMqPostman.Common.Interfaces
{
    public interface ILocalizerError
    {
        string BuildError(Exception ex);
        ApiException LocalizeTheError(Exception ex);
    }
}
