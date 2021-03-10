using System.Collections.Generic;
using System.Threading.Tasks;

using eShopSolution.Application.Catalog.Products.Dtos;
using eShopSolution.Application.Dtos;

using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;

namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;

        public ManageProductService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductEditRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<PageViewModel<ProductViewModel>> GetAllPaging(string keyword, int pageIndex, int pageSize)
        {
            throw new System.NotImplementedException();
        }
    }
}