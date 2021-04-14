namespace RebitMqPostman.BLL.Models
{
    public class RabbitMqConfiguration
    {
        public string QueueName { get; set; }
        public string Hostname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
    }
}
