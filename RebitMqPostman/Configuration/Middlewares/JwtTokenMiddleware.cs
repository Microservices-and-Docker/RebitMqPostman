using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RabbitMqPostman.Common.Models;
using RabbitMqPostman.Common.Interfaces;
using RabbitMqPostman.Interfaces;

namespace RabbitMqPostman.Configuration.Middlewares
{
    public class JwtTokenMiddleware
    {
        private RequestInfo _requestInfo;
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly IApiLogger _logger;

        public JwtTokenMiddleware(IApiLogger logger, RequestDelegate next,
                                  IOptions<AppSettings> appSettings, IOptions<RequestInfo> requestInfo)
        {
            _next = next;
            _logger = logger;
            _appSettings = appSettings.Value;
            _requestInfo = requestInfo.Value;
        }

        public async Task Invoke(HttpContext context, ILocalizer localizer)
        {
            var token = context.Request.Headers["Authorization"]
                                       .FirstOrDefault()?
                                       .Split(" ")
                                       .Last();
            try
            {
                if (token == null)
                    throw new ApiException(ErrorCodes.JwtToken_IsEmpty);

                ValidateJwtToken(context, token);

                await _next(context);
            }
            catch (Exception ex)
            {
                var err = localizer.BuildError(ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsync(err).ConfigureAwait(false);

                _logger.LogError(ex, err);
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
                throw new ApiException(ErrorCodes.JwtToken_SignatureValidationFailed, e.Message);
            }
            catch (SecurityTokenExpiredException e)
            {
                throw new ApiException(ErrorCodes.JwtToken_TokenIsExpired, e.Message);
            }
        }
    }
}
