using System;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using RabbitMqPostman.Common.Models;
using RabbitMqPostman.Interfaces;

namespace RabbitMqPostman.Infrastructure
{
    public class Localizer<T> : ILocalizer
    {
        protected readonly IStringLocalizer<T> _localizer;

        public Localizer(IStringLocalizer<T> localizer)
        {
            _localizer = localizer;
        }

        public ApiException LocalizeTheError(Exception ex)
        {
            string message;
            ApiException error = null;
            //todo create error response
            //todo transffere to RabbitMqPostman dll
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
