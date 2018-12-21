using Kts.RefactorThis.DTO;
using System.Collections.Generic;

namespace Kts.RefactorThis.Api.Payloads
{
    public class MappingProfileApi : AutoMapper.Profile
    {
        public MappingProfileApi()
        {
            CreateMap<QueryProductDTO, OutboundProduct>();
            CreateMap<QueryProductOptionDTO, OutboundProductOption>();

            CreateMap<IEnumerable<QueryProductDTO>, List<OutboundProduct>>();
            CreateMap<IEnumerable<QueryProductOptionDTO>, List<OutboundProductOption>>();

            CreateMap<InboundProduct, CreateProductDTO>();
            CreateMap<InboundProductOption, CreateProductOptionDTO>();

            CreateMap<InboundProduct, UpdateProductDTO>();
            CreateMap<InboundProductOption, UpdateProductOptionDTO>();
        }
    }
}
