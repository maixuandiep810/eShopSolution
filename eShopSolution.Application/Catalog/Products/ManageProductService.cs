using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using eShopSolution.Application.Catalog.Products.Dtos;
using eShopSolution.Application.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos.Manage;

using eShopSolution.Data.EF;
using Microsoft.EntityFrameworkCore;

using eShopSolution.Data.Entities;

using eShopSolution.Utilities.Exceptions;

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
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>(){
                    new ProductTranslation(){
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        SeoAlias = request.SeoAlias,
                        LanguageId = request.LanguageId
                    }
                }
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(p => p.ProductId == request.Id && p.LanguageId == request.LanguageId);

            if (product == null || productTranslation == null)
            {
                throw new EShopException("Cannot find a product with id: {productId}");
            }

            productTranslation.Name = request.Name;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.SeoAlias = request.SeoAlias;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EShopException("Cannot find a product with id: {productId}");
            }

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal price)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EShopException("Cannot find a product with id: {productId}");
            }
            product.Price = price;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EShopException("Cannot find a product with id: {productId}");
            }
            product.Stock += addedQuantity;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            //2. Filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }

            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(x => request.CategoryIds.Contains(x.pic.CategoryId));
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




/*
1. join thgi ra kq Linq ntn
*/
