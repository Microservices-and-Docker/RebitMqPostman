using System;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using RabbitMqPostman.Common.Interfaces;
using RabbitMqPostman.Common.Models;

namespace RabbitMqPostman.Common.Infrastructure
{
    public class LocalizerError<T> : ILocalizerError
    {
        protected readonly IStringLocalizer<T> _localizer;

        public LocalizerError(IStringLocalizer<T> localizer)
        {
            _localizer = localizer;
        }

        public ApiException LocalizeTheError(Exception ex)
        {
            string message;
            ApiException error = null;

            switch (ex)
            {
                case ApiException e:
                    message = _localizer[e.ErrorCode.ToString()] ?? e.ErrorCode.ToString();
                    error = new ApiException(e.ErrorCode, e.Reason, message);
                    break;

                default:
                    message = _localizer["Undefined"] ?? _localizer[ex.Message];
                    string reason = ex.InnerException?.Message ?? ex.Message;
                    error = new ApiException(ErrorCodes.Undefined, reason, message);
                    break;
            }

            return error;
        }

        public string BuildError(Exception ex)
            => JsonConvert.SerializeObject(LocalizeTheError(ex));
    }
}
