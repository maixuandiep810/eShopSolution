using Microsoft.AspNetCore.Mvc;
using eShopSolution.Application.Catalog.Products;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.Utilities.Exceptions;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;

        public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }


        // 
        // 
        //          READ
        // 
        // 


        // route: domain/api/products/
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var products = await _publicProductService.GetAll();
            return Ok(products);
        }

        // route: domain/api/products/vi-VN
        [HttpGet("{languageId:regex(\\D)}")]
        public async Task<IActionResult> Get(string languageId)
        {
            var products = await _publicProductService.GetAll(languageId);
            return Ok(products);
            // return Ok($"vi-VN: Chua trien khai -> {languageId}");
        }

        // route: domain/api/products/public-paging
        [HttpGet("public-paging")]
        public async Task<IActionResult> Get([FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(request);
            return Ok(products);
        }

        // route: domain/api/products/vn-VN/public-paging
        [HttpGet("{languageId:alpha}/public-paging")]
        public async Task<IActionResult> Get(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            return Ok($"vi-VN: Chua trien khai -> {languageId}");
        }


        // route: domain/api/products/1
        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetById(int productId)
        {
            try
            {
                var product = await _manageProductService.GetById(productId);
                return Ok(product);
            }
            catch (EShopException e)
            {
                return BadRequest(e.Message);
            }
        }


        // 
        // 
        //          CREATE - UPDATE - DELETE
        // 
        // 


        // route: POST:domain/api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var productId = await _manageProductService.Create(request);
            if (productId == 0)
            {
                return BadRequest();
            }
            else
            {
                var product = _manageProductService.GetById(productId);
                return Created(nameof(GetById), productId);
            }
        }


        // route: PUT:domain/api/products
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            try
            {
                var productId = _manageProductService.Update(request);
                return Ok(productId);
            }
            catch (EShopException e)
            {
                return BadRequest(e.Message);
            }

        }


        // route: DELETE:domain/api/products/1
        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                int numDeletedRecord = await _manageProductService.Delete(productId);
                return Ok(numDeletedRecord);
            }
            catch (EShopException e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}