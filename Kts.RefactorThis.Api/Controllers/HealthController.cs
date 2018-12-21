using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Kts.RefactorThis.Api.Filters;
using Kts.RefactorThis.Api.Payloads;
using Kts.RefactorThis.Common;

namespace Kts.RefactorThis.Api.Controllers
{
    [Route("/health")]
    [Produces("application/json")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private AppConfiguration _appConfigs;
        private readonly IMapper _mapper;

        public HealthController(AppConfiguration appConfigs, 
                                 IMapper mapper)
        {
            _appConfigs = appConfigs;
            _mapper = mapper;
        }

        /// <summary>
        /// Ping test
        /// </summary>
        /// <response code="200"></response>
        [HttpGet("ping")]
        [ProducesResponseType(200, Type = typeof(OutboundProducts))]
        [ProducesResponseType(400)]
        [ServiceFilter(typeof(PaginationFilter))]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
