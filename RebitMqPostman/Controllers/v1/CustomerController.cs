using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using RebitMqPostman.BLL.Models;
using RebitMqPostman.BLL.v1.Handlers.Comands;
using RebitMqPostman.Models.v1;

namespace RebitMqPostman.Controllers.v1
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Customer>> Index(CreateCustomerModel createCustomerModel)
        {
            return await _mediator.Send(new CreateCustomerCommand
            {
                Customer = _mapper.Map<Customer>(createCustomerModel)
            });
        }
    }
}
