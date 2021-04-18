using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RabbitMqPostman.Common.Models;
using Microsoft.Extensions.Logging;

namespace RabbitMqPostman.Configuration.Middlewares
{
    public class JwtTokenMiddleware
    {
        private RequestInfo _requestInfo;
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly ILogger<JwtTokenMiddleware> _logger;

        public JwtTokenMiddleware(ILogger<JwtTokenMiddleware> logger, RequestDelegate next,
                                  IOptions<AppSettings> appSettings, IOptions<RequestInfo> requestInfo)
        {
            _next = next;
            _logger = logger;
            _appSettings = appSettings.Value;
            _requestInfo = requestInfo.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            try
            {
                if (token == null)
                    throw new Exception("JwtToken is null");

                ValidateJwtToken(context, token);

                await _next(context);
            }
            catch (Exception e)
            {
                //todo add localization error
                //var err = localizer.BuildError(ex.Error);
                // _logger.LogInformation(new EventId(0, _requestInfo.CorrelationId.ToString()), e, err);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                //   await context.Response.WriteAsync(err).ConfigureAwait(false);
            }
        }

        private void ValidateJwtToken(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtTokenKey);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken = null;
                var claims = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                _requestInfo.UserInfo = new UserInfo
                {
                    DateCreatedToken = validatedToken.ValidFrom,
                    UserId = claims.FindFirst("UserId")?.Value,
                    AppId = claims.FindFirst("AppId")?.Value,
                    PhoneNumber = claims.FindFirst("Phone")?.Value
                };
            }
            catch (SecurityTokenInvalidSignatureException e)
            {
                throw new RabbitMqPostmanException(ErrorCodes.JwtToken_SignatureValidationFailed, e.Message);
            }
            catch (SecurityTokenExpiredException e)
            {
                throw new RabbitMqPostmanException(ErrorCodes.JwtToken_TokenIsExpired, e.Message);
            }
        }
    }
}
