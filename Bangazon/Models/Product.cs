using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bangazon.Models
{
    public class Product
  {
    [Key]
    public int ProductId {get;set;}

    private double _price;

    [Required]
    [DataType(DataType.Date)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated {get;set;}

    [Required]
    [StringLength(255)]
    public string Description { get; set; }

    [Required]
    [StringLength(55, ErrorMessage="Please shorten the product title to 55 characters")]
    public string Title { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [Range(1, 10000)]
    public double Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    public string City { get; set; }

    [Required]
    public ApplicationUser User { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required(ErrorMessage = "Please select a Product Category")]
    [Range(1, double.MaxValue, ErrorMessage = "Please select a Product Category")]
    [Display(Name="Product Category")]
    public int ProductTypeId { get; set; }

    [Required(ErrorMessage = "Please select a Product Category")]
    public ProductType ProductType { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; }

  }
}
