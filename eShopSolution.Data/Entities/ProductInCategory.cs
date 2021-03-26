using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class ProductInCategory
    {
        public int ProductId { get; set; }


        public int CategoryId { get; set; }

        public virtual Product Product { get; set; }


        public virtual Category Category { get; set; }
    }
}
