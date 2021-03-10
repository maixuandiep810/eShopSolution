using System.Collections.Generic;
namespace eShopSolution.Application.Dtos
{
    public class PageViewModel<T>
    {
        public List<T> items { set; get; }
        public int TotalRecord { set; get; }
    }
}