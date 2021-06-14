using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ACME_INC_APP.Models
{
    [Table("ProdCategory")]
    public partial class ProdCategory
    {
        public ProdCategory()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("ProdCatID")]
        public int ProdCatId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProdCatName { get; set; }

        [InverseProperty(nameof(Product.ProdCat))]
        public virtual ICollection<Product> Products { get; set; }
    }
}
