using Microsoft.AspNetCore.Mvc;
using eShopSolution.Application.Catalog.Products;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.Utilities.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        //          PRODUCT
        //          READ
        // 
        //

        //          READ ALL

        // ROUTE: domain/api/products/vi-VN   
        [HttpGet("{languageId:regex(\\D)}")]
        // [HttpGet("{languageId:regex(\\D):regex([[^(all-lang)]])}")]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var products = await _publicProductService.GetAll(languageId);
            return Ok(products);
            // return Ok($"vi-VN: Chua trien khai -> {languageId}");
        }

        // ROUTE: domain/api/products/all-lang
        [HttpGet("all-lang")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _publicProductService.GetAll();
            return Ok(products);
        }

        //          PAGING

        // ROUTE: domain/api/products/vi-VN/public-paging?pagesize=2&pageindex=1&categoryid=1
        [HttpGet("{languageId:alpha}/public-paging")]
        public async Task<IActionResult> Get(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var pageResultProducts = await _publicProductService.GetAllByCategoryId(languageId, request);
            return Ok(pageResultProducts);
            // return Ok($"vi-VN: Chua trien khai -> {languageId}");
        }

        // ROUTE: domain/api/products/all-lang/public-paging?pagesize=2&pageindex=1&categoryid=1
        [HttpGet("all-lang/public-paging")]
        public async Task<IActionResult> Get([FromQuery] GetPublicProductPagingRequest request)
        {
            var pageResultProducts = await _publicProductService.GetAllByCategoryId(request);
            return Ok(pageResultProducts);
        }

        //          BY ID

        // ROUTE: domain/api/products/all-lang/1
        [HttpGet("all-lang/{productId:int}")]
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
        //          PRODUCT
        //          CREATE - UPDATE - DELETE
        // 
        // 


        // ROUTE: POST:domain/api/products/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var productId = await _manageProductService.Create(request);
            if (productId == 0)
            {
                return BadRequest();
            }
            else
            {
                var product = await _manageProductService.GetById(productId);
                return Created(nameof(GetById), productId);
            }
        }


        // ROUTE: PUT:domain/api/products/updateinfo
        [HttpPut("updateinfo")]
        public async Task<IActionResult> UpdateInfo([FromForm] ProductUpdateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var productId = await _manageProductService.Update(request);
                return Ok(productId);
            }
            catch (EShopException e)
            {
                return BadRequest(e.Message);
            }

        }

        // ROUTE: PUT:domain/api/products/updateprice
        [HttpPatch("updateprice")]
        public async Task<IActionResult> UpdatePrice([FromForm] ProductPriceUpdateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updateResult = await _manageProductService.UpdatePrice(request);
                return Ok(updateResult);
            }
            catch (EShopException e)
            {
                return BadRequest(e.Message);
            }

        }


        // ROUTE: DELETE:domain/api/products/1
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



        // 
        // 
        //          IMAGE
        //          CREATE - READ - UPDATE - DELETE 
        // 
        // 

        // ROUTE: POST:domain/api/products/1/images/create
        [HttpPost("{productid}/images/create")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var imageId = await _manageProductService.AddImage(productId, request);
            if (imageId == 0)
            {
                return BadRequest();
            }
            else
            {
                var image = await _manageProductService.GetImageById(productId);
                return Created(nameof(GetById), imageId);
            }
        }

        // ROUTE: PUT:domain/api/products/1/images/updateinfo
        [HttpPut("{productid}/images/updateinfo")]
        public async Task<IActionResult> UpdateImage(int productId, [FromForm] ProductImageUpdateRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var result = await _manageProductService.UpdateImage(productId, request);
            if (result == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        // ROUTE: DELETE:domain/api/products/1/images/1
        [HttpDelete("{productid}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int productId, int imageId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var result = await _manageProductService.RemoveImage(imageId);
            if (result == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        // ROUTE: GET:domain/api/products/1/images/1
        [HttpGet("{productid}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _manageProductService.GetImageById(imageId);
            return Ok(image);
        }
    }
}