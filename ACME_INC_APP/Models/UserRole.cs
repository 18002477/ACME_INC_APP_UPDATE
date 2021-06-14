using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ACME_INC_APP.Models
{
    [Table("UserRole")]
    public partial class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [Column("UserRoleID")]
        public int UserRoleId { get; set; }
        [Required]
        [Column("UserRole")]
        [StringLength(50)]
        public string UserRole1 { get; set; }

        [InverseProperty(nameof(User.UserRole))]
        public virtual ICollection<User> Users { get; set; }
    }
}
