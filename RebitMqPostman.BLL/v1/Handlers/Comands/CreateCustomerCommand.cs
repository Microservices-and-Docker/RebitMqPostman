using MediatR;
using RabbitMqPostman.BLL.Models;

namespace RabbitMqPostman.BLL.v1.Handlers.Comands
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public Customer Customer { get; set; }
    }
}
