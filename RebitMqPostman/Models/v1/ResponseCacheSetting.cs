using Microsoft.AspNetCore.Mvc;

namespace RabbitMqPostman.Models.v1
{
    public class ResponseCacheSetting
    {
        public ResponseCacheSetting()
        {
            Duration = 30;
            Location = ResponseCacheLocation.Any;
            NoStore = false;
        }
        /// <summary>имя профиля кэширования </summary>
        public string CacheName { get; set; }

        /// <summary> максимальное время кэширования в секундах </summary>
        public int Duration { get; set; }

        /// <summary> определяет место кэширования </summary>
        public ResponseCacheLocation Location { get; set; }

        /// <summary> определяет, будет ли ответ кэшироваться. Если равен true, то ответ не кэшируется </summary>       
        public bool NoStore { get; set; }
    }
}
