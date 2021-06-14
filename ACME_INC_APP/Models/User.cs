using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ACME_INC_APP.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
        }

        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Must be between 3 and 16 characters", MinimumLength = 3)]
        public string Username { get; set; }
        [Display(Name = "Email address")]
        [StringLength(50)]
        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Column("UserRoleID")]
        public int UserRoleId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = " The Password must at least have 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Column("VCode")]
        [StringLength(50)]
        public string Vcode { get; set; }

        [ForeignKey(nameof(UserRoleId))]
        [InverseProperty("Users")]
        public virtual UserRole UserRole { get; set; }
        [InverseProperty(nameof(Cart.User))]
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
