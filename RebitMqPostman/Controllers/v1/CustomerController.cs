using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using RabbitMqPostman.BLL.Models;
using RabbitMqPostman.BLL.v1.Handlers.Comands;
using RabbitMqPostman.Models.v1;

namespace RabbitMqPostman.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CustomerController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Action to create a new customer in the database.
        /// </summary>
        /// <param name="createCustomerModel">Model to create a new customer</param>
        /// <returns>Returns the created customer</returns>
        /// <response code="200">Returned if the customer was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or the customer couldn't be saved</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost]
        public async Task<ActionResult<Customer>> Index(CreateCustomerModel createCustomerModel)
        {
            return await _mediator.Send(new CreateCustomerCommand
            {
                Customer = _mapper.Map<Customer>(createCustomerModel)
            });
        }
    }
}
