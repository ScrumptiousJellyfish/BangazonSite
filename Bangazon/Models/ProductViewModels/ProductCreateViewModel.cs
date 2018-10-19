using Bangazon.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Bangazon.Models.ProductViewModels
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public List<SelectListItem> ProductTypes { get; }

        public ProductCreateViewModel() { }

        public ProductCreateViewModel(ApplicationDbContext context)
        {
            ProductTypes = context.ProductType.Select(productType =>
            new SelectListItem { Text = productType.Label, Value = productType.ProductTypeId.ToString() }).ToList();

            //Add a prompt so that the<select> element isn't blank
            this.ProductTypes.Insert(0, new SelectListItem
            {
                Text = "Choose Product Type...",
                Value = "0"
            });
        }
    }
}
