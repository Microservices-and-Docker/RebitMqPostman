namespace RebitMqPostman.BLL.Interfaces
{
    public interface IRebitMqSender
    {
        void SendMessage(object message);
    }
}
