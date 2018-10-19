using Bangazon.Models;
using Bangazon.Data;

namespace Bangazon.Models.ProductViewModels
{
  public class ProductDetailViewModel
  {
        public Product Product { get; set; }

        public ProductDetailViewModel(Product product)
        {
            Product = product;
        }
    }
}