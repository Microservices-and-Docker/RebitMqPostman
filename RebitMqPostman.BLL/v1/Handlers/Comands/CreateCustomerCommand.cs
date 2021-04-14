using MediatR;
using RebitMqPostman.BLL.Models;

namespace RebitMqPostman.BLL.v1.Handlers.Comands
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public Customer Customer { get; set; }
    }
}
