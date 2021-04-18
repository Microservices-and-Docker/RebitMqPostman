using System;
using Microsoft.Extensions.Options;
using RabbitMqPostman.BLL.Models;
using RabbitMqPostman.BLL.RebitMq;

namespace RabbitMqPostman.BLL.v1.Services
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
            // var customer = await _customerRepository.AddAsync(request.Customer);

            throw new NotImplementedException();
        }
    }
}
