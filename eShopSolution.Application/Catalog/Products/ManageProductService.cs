using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IO;
using eShopSolution.Application.Common;
using eShopSolution.ViewModels.Catalog.ProductImages;

namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;

        public ManageProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }



        //
        //
        //                  PRODUCT
        //                  CREATE - UPDATE - DELETE
        //
        //

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

            //5. Save image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(p => p.ProductId == request.Id && p.LanguageId == request.LanguageId);

            if (product == null || productTranslation == null)
            {
                string message = $"Cannot find a product with id: {request.Id}";
                throw new EShopException(message);
            }

            productTranslation.Name = request.Name;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.SeoAlias = request.SeoAlias;

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                string message = $"Cannot find a product with id: {productId}";
                throw new EShopException(message);
            }

            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(ProductPriceUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                string message = $"Cannot find a product with id: {request.Id}";
                throw new EShopException(message);
            }
            product.Price = request.Price;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                string message = $"Cannot find a product with id: {productId}";
                throw new EShopException(message);
            }
            product.Stock += addedQuantity;

            return await _context.SaveChangesAsync() > 0;
        }


        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                string message = $"Cannot find a product with id: {productId}";
                throw new EShopException(message);
            }
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }



        //
        //
        //                  PRODUCT
        //                  READ
        //
        //

        public async Task<Product> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                string message = $"Cannot find a product with id: {productId}";
                throw new EShopException(message);
            }
            return product;
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);

            if (product == null || productTranslation == null)
            {
                string message = $"Cannot find a product with id: {productId}";
                throw new EShopException(message);
            }

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount
            };
            return productViewModel;
        }


        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
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
                Items = data,
                TotalRecord = totalRow,
            };
            return pageResult;
        }




        // 
        //
        //                  IMAGE
        //
        //

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new EShopException($"Cannot find an image with id {imageId}");
            }
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int productId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(request.Id);
            if (productImage == null)
            {
                throw new EShopException($"Cannot find an image with id {request.Id}");
            }
r3
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new EShopException($"Cannot find an image with id {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }



        //
        //
        //                  PRIVATE TASK
        //
        //

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}




/*
1. join thgi ra kq Linq ntn
*/
