using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ACME_INC_APP.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
        }

        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [Column("ProdCatID")]
        public int ProdCatId { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }
        [Required]
        public byte[] ProductImage { get; set; }

        [ForeignKey(nameof(ProdCatId))]
        [InverseProperty(nameof(ProdCategory.Products))]
        public virtual ProdCategory ProdCat { get; set; }
        [InverseProperty(nameof(Cart.Product))]
        public virtual ICollection<Cart> Carts { get; set; }
        [InverseProperty(nameof(Order.Product))]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
