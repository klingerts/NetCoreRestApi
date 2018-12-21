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
    [Route("/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private AppConfiguration _appConfigs;
        private readonly IMapper _mapper;

        private IProductQueryHandler _productQueryHandler;
        private IProductService _productService;

        public ProductsController(AppConfiguration appConfigs, 
                                  IMapper mapper, 
                                  IProductQueryHandler productQueryHandler,
                                  IProductService productService)
        {
            _appConfigs = appConfigs;
            _mapper = mapper;

            _productQueryHandler = productQueryHandler;
            _productService = productService;
        }

        /// <summary>
        /// Get products
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <![CDATA[
        ///     GET /Products?name=Item&offset=0&limit=10
        /// ]]>
        /// </remarks>
        /// <param name="name">Optional filter argument. Must be exact.</param>
        /// <param name="pagination">Optional pagination arguments</param>
        /// <returns>List of products</returns>
        /// <response code="200">Returns list of products</response>
        /// <response code="400">If arguments are invalid</response>    
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(OutboundProducts))]
        [ProducesResponseType(400)]
        [ServiceFilter(typeof(PaginationFilter))]
        public async Task<IActionResult> Get([FromQuery] string name, [FromQuery] PaginationParams pagination)
        {
            var products = await _productQueryHandler.GetProductsAsync(pagination, name);
            var payload = new OutboundProducts { Items = _mapper.Map<IEnumerable<OutboundProduct>>(products) };

            return Ok(payload);
        }

        /// <summary>
        /// Get single product
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Products/{id}
        ///     
        /// </remarks>
        /// <param name="id">Product unique identifier</param>
        /// <returns>Product for id</returns>
        /// <response code="200">Product</response>
        /// <response code="404">When product not found</response> 
        [HttpGet("{id:guid}")]
        [ProducesResponseType(200, Type = typeof(OutboundProduct))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var product = await _productQueryHandler.GetProductByIdAsync(id);

            if (product == null) return new NotFoundObjectResult(new { id });

            var payload = _mapper.Map<OutboundProduct>(product);

            return Ok(payload);
        }

        /// <summary>
        /// Post product
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Products
        ///     {
        ///        "name": "Item1",
        ///        "Description": "Description",
        ///        "Price": 150.12,
        ///        "DeliveryPrice": 1.50
        ///     }
        ///     
        /// </remarks>
        /// <param name="payload">Product to create</param>
        /// <returns>New product id</returns>
        /// <response code="201">New product Id</response>
        /// <response code="400">Invalid data</response> 
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400, Type = typeof(ProblemDetailsWithValidation))]
        public async Task<IActionResult> Post([FromBody] InboundProduct payload)
        {
            var productToAdd = _mapper.Map<CreateProductDTO>(payload);
            var result = await _productService.CreateAsync(productToAdd);
            
            if (result.Success)
            {
                return CreatedAtAction(nameof(Get), new { id = result.Data }, new { id = result.Data });
            }

            return new BadRequestObjectResult(ModelState.AsProblemDetail(result));
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Products/{id}
        ///     {
        ///        "Name": "Item1",
        ///        "Description": "Description",
        ///        "Price": 150.12,
        ///        "DeliveryPrice": 1.50
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Product id</param>
        /// <param name="payload">Data to update</param>
        /// <returns></returns>
        /// <response code="204">Product updated</response>
        /// <response code="400">Invalid data</response> 
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(ProblemDetailsWithValidation))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] InboundProduct payload)
        {
            var productToUpdate = _mapper.Map<UpdateProductDTO>(payload);
            productToUpdate.Id = id;
            var result = await _productService.UpdateAsync(productToUpdate);

            if (result.Success)
            {
                if (result.Data) return NoContent();
                else return new NotFoundObjectResult(new { id });
            }

            return new BadRequestObjectResult(ModelState.AsProblemDetail(result));
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /Products/{id}
        ///     
        /// </remarks>
        /// <response code="204">Product deleted</response>
        /// <response code="404">Product not found</response> 
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _productService.DeleteAsync(id);

            if (result.Success && result.Data)
            {
                return NoContent();
            }

            return new NotFoundObjectResult(new { id });
        }
    }
}
