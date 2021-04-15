using System;
using Microsoft.Extensions.Options;
using RebitMqPostman.BLL.Models;
using RebitMqPostman.BLL.RebitMq;

namespace RebitMqPostman.BLL.v1.Services
{
    public class RabbitMqNewCustomerListener : RabbitMqListener<Customer>
    {
        //  private readonly ICustomerRepository _customerRepository;
        public RabbitMqNewCustomerListener(IOptions<RabbitMqConfiguration> rabbitMqOptions) //, ICustomerRepository customerRepository
            : base(rabbitMqOptions.Value)
        {
            // _customerRepository = customerRepository;
        }

        public override void HandleMessage(Customer message)
        {
            //to do add loggerFactory
            // var customer = await _customerRepository.AddAsync(request.Customer);

            throw new NotImplementedException();
        }
    }
}
