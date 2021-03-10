using eShopSolution.Application.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageViewModel<ProductViewModel>> GetAllByCategoryId(int categoryId, int pageIndex, int pageSize);
    }
}