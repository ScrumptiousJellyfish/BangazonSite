using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.ProductTypeViewModels
{
    public class ProductTypeViewModel
    {
        public ProductType productType { get; set; }

        public List<Product> products { get; set; }
    }
}
