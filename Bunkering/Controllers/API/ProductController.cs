using Bunkering.Access.Services;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ResponseController
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;

        }

        /// <summary>
        /// This endpoint is used to create product
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/product/create-products
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("create-products")]
        [HttpPost]

        public async Task<IActionResult> CreateProduct(ProductViewModel model) => Response(await _productService.CreateProduct(model));



        /// <summary>
        /// This endpoint is used to edit product
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/product/edit-product
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("edit-products")]
        [HttpPost]

        public async Task<IActionResult> EditProduct(ProductViewModel model) => Response(await _productService.EditProduct(model));



        /// <summary>
        /// This endpoint is used to GET ALL product
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/product/all-product
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("all-products")]
        [HttpGet]

        public async Task<IActionResult> AllProduct() => Response(await _productService.AllProducts());




        /// <summary>
        /// This endpoint is used to GET ALL product types
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/product/all-product-types
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("all-product-types")]
        [HttpGet]
        public IActionResult GetProductTypes()
        {
            var productTypes = Enum.GetNames(typeof(ProductTypes));
            return Ok(new ApiResponse
            {
               Data = productTypes,
               Message = "Successful",
               Success = true
            });
        }

    }
}
