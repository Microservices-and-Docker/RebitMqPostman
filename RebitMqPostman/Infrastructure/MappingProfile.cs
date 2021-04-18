using AutoMapper;
using RabbitMqPostman.BLL.Models;
using RabbitMqPostman.Models.v1;

namespace RabbitMqPostman.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerModel, Customer>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
