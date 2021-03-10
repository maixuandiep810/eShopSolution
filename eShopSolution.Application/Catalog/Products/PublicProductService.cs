using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.Data.EF;

namespace eShopSolution.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {

        private readonly EShopDbContext _context;

        public PublicProductService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            //2. Filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            //3. Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                        .Select(x => new ProductViewModel()
                        {
                            Id = x.p.Id,
                            DateCreated = x.p.DateCreated,
                            Price = x.p.Price,
                            OriginalPrice = x.p.OriginalPrice,
                            Stock = x.p.Stock,
                            ViewCount = x.p.ViewCount,
                            Name = x.pt.Name,
                            Description = x.pt.Description,
                            Details = x.pt.Details,
                            SeoDescription = x.pt.SeoDescription,
                            SeoAlias = x.pt.SeoAlias,
                            SeoTitle = x.pt.SeoTitle
                        })
                        .ToListAsync();

            //4. Select and project
            var pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }
 
    }
}