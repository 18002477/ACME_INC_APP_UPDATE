using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ACME_INC_APP.Models
{
    public partial class Order
    {
        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("Orders")]
        public virtual Product Product { get; set; }
    }
}
