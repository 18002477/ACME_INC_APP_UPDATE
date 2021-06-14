using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ACME_INC_APP.Models
{
    [Table("Cart")]
    public partial class Cart
    {
        [Key]
        [Column("CartID")]
        public int CartId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("Carts")]
        public virtual Product Product { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Carts")]
        public virtual User User { get; set; }
    }
}
