using System.Threading;
using System.Threading.Tasks;
using RebitMqPostman.BLL.Interfaces;
using RebitMqPostman.BLL.Models;
using RebitMqPostman.BLL.v1.Handlers.Comands;

namespace RebitMqPostman.BLL.v1.Handlers
{
    public class MessageHendler
    {
        private readonly IRebitMqSender _rebitMqSender;
      //  private readonly ICustomerRepository _customerRepository;

        public MessageHendler(IRebitMqSender rebitMqSender) //, ICustomerRepository customerRepository
        {
            _rebitMqSender = rebitMqSender;
           // _customerRepository = customerRepository;
        }


        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
             // var customer = await _customerRepository.AddAsync(request.Customer);

            _rebitMqSender.SendMessage(request.Customer);

            return request.Customer;
        }
    }
}
