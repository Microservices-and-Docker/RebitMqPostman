using System;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using RabbitMqPostman.Common.Models;
using RabbitMqPostman.Interfaces;
using RabbitMqPostman.Models.v1;

namespace RabbitMqPostman.Infrastructure
{
    public class Localizer<T> : ILocalizer
    {
        protected readonly IStringLocalizer<T> _localizer;

        public Localizer(IStringLocalizer<T> localizer)
        {
            _localizer = localizer;
        }

        public ErrorResponse LocalizeTheError(Exception ex)
        {
            string message;
            ErrorResponse error = null;

            switch (ex)
            {
                case ApiException e:
                    message = _localizer[e.ErrorCode.ToString()] ?? e.ErrorCode.ToString();
                    error = new ErrorResponse(message, e.Reason);
                    break;

                default:
                    message = _localizer["Undefined"] ?? _localizer[ex.Message];
                    string reason = ex.InnerException?.Message ?? ex.Message;
                    error = new ErrorResponse(message, reason);
                    break;
            }

            return error;
        }

        public string BuildError(Exception ex)
            => JsonConvert.SerializeObject(LocalizeTheError(ex));
    }
}
