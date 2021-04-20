using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqPostman.Models.v1;

namespace RabbitMqPostman.Configuration.Services
{
    public static class ResponseCacheExtension
    {
        /// <summary> 
        /// Добавить настройки кеширование ответов из файла appsettings.json 
        /// Для добавления кеша на метод контролера надо указать атрибут и имя кеша [ResponseCache(CacheProfileName = "CachingName")]
        /// </summary>
        public static void AddResponseCaches(this IServiceCollection services, IConfiguration configuration)
        {
            var responseCachesSetting = new List<ResponseCacheSetting>();
            configuration.GetSection("ResponseCachesSetting").Bind(responseCachesSetting);

            foreach (var cacheSetting in responseCachesSetting)
            {
                services.AddMvcCore(options =>
                {
                    options.CacheProfiles.Add(cacheSetting.CacheName,
                        new CacheProfile()
                        {
                            Duration = cacheSetting.Duration,
                            Location = cacheSetting.Location,
                            NoStore = cacheSetting.NoStore
                        });
                });
            }

            services.AddResponseCaching();
        }
    }
}
