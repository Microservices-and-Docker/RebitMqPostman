using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RabbitMqPostman.BLL.Interfaces;
using RabbitMqPostman.BLL.Models;
using RabbitMqPostman.BLL.v1.Handlers.Comands;
using RabbitMqPostman.Common.Interfaces;
using RabbitMqPostman.Common.Models;

namespace RabbitMqPostman.BLL.v1.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly IApiLogger _logger;
        private readonly IRebitMqSender _rebitMqSender;

        public CreateCustomerCommandHandler(IApiLogger logger, IRebitMqSender rebitMqSender)
        {
            _logger = logger;
            _rebitMqSender = rebitMqSender;
        }

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _rebitMqSender.SendMessage(request.Customer);

                return request.Customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Handle CreateCustomer");

                throw new ApiException(ErrorCodes.Error_HandleCreateCustomer);
            }
        }
    }
}
