using Microsoft.AspNetCore.Http;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductPriceUpdateRequest
    {
        public int Id { set; get; }
        public int Price { set; get; }
    }
}

