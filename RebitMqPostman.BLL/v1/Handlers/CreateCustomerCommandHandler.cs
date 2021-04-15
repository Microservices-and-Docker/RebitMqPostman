using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RebitMqPostman.BLL.Interfaces;
using RebitMqPostman.BLL.Models;
using RebitMqPostman.BLL.v1.Handlers.Comands;

namespace RebitMqPostman.BLL.v1.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly IRebitMqSender _rebitMqSender;

        public CreateCustomerCommandHandler(IRebitMqSender rebitMqSender)
        {
            _rebitMqSender = rebitMqSender;
        }


        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            _rebitMqSender.SendMessage(request.Customer);

            return request.Customer;
        }
    }
}
