using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.ProductTypesViewModels
{
    public class ProductTypesViewModel
    {
        public IEnumerable<Product> Product { get; set; }

        public ProductType ProductType { get; set; }
    }
}