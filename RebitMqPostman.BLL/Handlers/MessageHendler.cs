using RebitMqPostman.BLL.Interfaces;
using System.Threading.Tasks;

namespace RebitMqPostman.BLL.Handlers
{
    public class MessageHendler
    {
       private readonly IRebitMqSender _rebitMqSender;

        public MessageHendler(IRebitMqSender rebitMqSender)
        {
            _rebitMqSender = rebitMqSender;
        }

        /*
        public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
          //  var customer = await _customerRepository.UpdateAsync(request.Customer);

            _rebitMqSender.SendMessage(customer);

            return customer;
        }
        */
    }
}
