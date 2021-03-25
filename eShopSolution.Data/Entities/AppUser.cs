using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Dob { get; set; }

        public virtual List<Cart> Carts { get; set; }

        public virtual List<Order> Orders { get; set; }

        public virtual List<Transaction> Transactions { get; set; }
    }
}
