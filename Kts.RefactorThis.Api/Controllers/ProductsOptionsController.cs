using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Kts.RefactorThis.Api.Filters;
using Kts.RefactorThis.Api.Payloads;
using Kts.RefactorThis.Application.Queries;
using Kts.RefactorThis.Common;
using Kts.RefactorThis.DTO;
using Kts.RefactorThis.Application.Services;
using Kts.RefactorThis.Api.ErrorHandling;

namespace Kts.RefactorThis.Api.Controllers
{
    [Route("/products/{id:guid}/options")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsOptionsController : ControllerBase
    {
        private AppConfiguration _appConfigs;
        private readonly IMapper _mapper;

        private IProductOptionQueryHandler _productOptionQueryHandler;
        private IProductOptionService _productOptionService;

        public ProductsOptionsController(AppConfiguration appConfigs, IMapper mapper,
                                         IProductOptionQueryHandler productOptionQueryHandler,
                                         IProductOptionService productOptionService)
        {
            _appConfigs = appConfigs;
            _mapper = mapper;

            _productOptionQueryHandler = productOptionQueryHandler;
            _productOptionService = productOptionService;
        }

        /// <summary>
        /// Get productOptions for a product
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Products/{id}/options
        ///     
        /// </remarks>
        /// <param name="id">Product id</param>
        /// <param name="pagination">Optional pagination arguments</param>
        /// <returns>List of productOptions</returns>
        /// <response code="200">ProductOptions list</response>
        /// <response code="400">Invalid arguments</response>    
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(OutboundProductOptions))]
        [ProducesResponseType(400)]
        [ServiceFilter(typeof(PaginationFilter))]
        public async Task<IActionResult> GetByProductId([FromRoute] Guid id, [FromQuery] PaginationParams pagination)
        {
            var options = await _productOptionQueryHandler.GetProductOptionsByProductIdAsync(pagination, id);

            if (options == null) return new NotFoundObjectResult(new { id });

            var payload = new OutboundProductOptions { Items = _mapper.Map<IEnumerable<OutboundProductOption>>(options) };

            return Ok(payload);
        }

        /// <summary>
        /// Get productOption
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Products/{id}/options/{optionId}
        ///     
        /// </remarks>
        /// <param name="id">Product id</param>
        /// <param name="optionId">ProductOption id</param>
        /// <returns>ProductOption data</returns>
        /// <response code="200">ProductOptions</response>
        /// <response code="404">ProductOption not found</response>   
        [HttpGet("{optionId:guid}")]
        [ProducesResponseType(200, Type = typeof(OutboundProductOption))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByProductIdAndOptionId([FromRoute] Guid id, [FromRoute] Guid optionId)
        {
            var option = await _productOptionQueryHandler.GetProductOptionAsync(id, optionId);

            if (option == null) return new NotFoundObjectResult(new { id, optionId });

            var payload = _mapper.Map<OutboundProductOption>(option);
            return Ok(payload);
        }

        /// <summary>
        /// Post product
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Products/{id}/Options
        ///     {
        ///        "name": "Item1",
        ///        "Description": "Description",
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">Product id</param>
        /// <param name="payload">ProductOption to create</param>
        /// <returns>New ProductOption id</returns>
        /// <response code="201">New productOption Id</response>
        /// <response code="400">Invalid data</response> 
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400, Type = typeof(ProblemDetailsWithValidation))]
        public async Task<IActionResult> Post([FromRoute] Guid id, [FromBody] InboundProductOption payload)
        {
            var productOptionToAdd = _mapper.Map<CreateProductOptionDTO>(payload);
            productOptionToAdd.ProductId = id;
            var result = await _productOptionService.CreateAsync(productOptionToAdd);

            if (result.Success)
            {
                return CreatedAtAction(nameof(GetByProductId), new { id, optionId = result.Data }, new { optionId = result.Data });
            }

            return new BadRequestObjectResult(ModelState.AsProblemDetail(result));
        }

        /// <summary>
        /// Update productOption
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Products/{id}/options/{optionId}
        ///     {
        ///        "name": "Item1",
        ///        "Description": "Description",
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Product id</param>
        /// <param name="optionId">ProductOption id</param>
        /// <param name="payload">Data to update</param>
        /// <returns></returns>
        /// <response code="204">ProductOption updated</response>
        /// <response code="400">Invalid data</response> 
        [HttpPut("{optionId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(ProblemDetailsWithValidation))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromRoute] Guid optionId, 
                                             [FromBody] InboundProductOption payload)
        {
            var productOptionToUpdate = _mapper.Map<UpdateProductOptionDTO>(payload);
            productOptionToUpdate.ProductId = id;
            productOptionToUpdate.OptionId = optionId;
            var result = await _productOptionService.UpdateAsync(productOptionToUpdate);

            if (result.Success)
            {
                if (result.Data) return NoContent();
                else return new NotFoundObjectResult(new { id, optionId });
            }

            return new BadRequestObjectResult(ModelState.AsProblemDetail(result));
        }

        /// <summary>
        /// Delete productOption
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /Products/{id}/options/optionId
        ///     
        /// </remarks>
        /// <response code="204">ProductOption deleted</response>
        /// <response code="404">ProductOption not found</response> 
        [HttpDelete("{optionId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromRoute] Guid optionId)
        {
            var result = await _productOptionService.DeleteAsync(id, optionId);

            if (result.Success && result.Data)
            {
                return NoContent();
            }

            return new NotFoundObjectResult(new { id, optionId });
        }
    }
}
