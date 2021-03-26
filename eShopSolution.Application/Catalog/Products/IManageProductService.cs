using System.Collections.Generic;
using System.Threading.Tasks;

using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Common;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IManageProductService
    {
        // PRODUCT
        // CUD
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<bool> UpdatePrice(ProductPriceUpdateRequest request);
        Task<bool> UpdateStock(int productId, int addedQuantity);
        Task AddViewCount(int productId);
        Task<int> Delete(int productId);

        // PRODUCT
        // R
        Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);
        Task<ProductViewModel> GetById(int productId, string languageId);
        Task<Product> GetById(int productId);


        // IMAGE
        // CRUD
        Task<int> AddImage(int productId, ProductImageCreateRequest request);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);
        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<List<ProductImageViewModel>> GetListImages(int productId);
    }
}