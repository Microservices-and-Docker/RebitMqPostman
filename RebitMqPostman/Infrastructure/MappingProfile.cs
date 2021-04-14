using AutoMapper;
using RebitMqPostman.BLL.Models;
using RebitMqPostman.Models.v1;

namespace RebitMqPostman.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerModel, Customer>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
