using System.Collections.Generic;
using System.Threading.Tasks;

using eShopSolution.Application.Catalog.Products.Dtos;
using eShopSolution.Application.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos.Manage;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);
        Task<bool> UpdatePrice(int productId, decimal price);
        Task<bool> UpdateStock(int productId, int addedQuantity);
        Task AddViewCount(int productId);
        Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
    }
}