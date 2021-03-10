using System.Collections.Generic;
namespace eShopSolution.Application.Dtos
{
    public class PageResult<T>
    {
        public List<T> Items { set; get; }
        public int TotalRecord { set; get; }
    }
}