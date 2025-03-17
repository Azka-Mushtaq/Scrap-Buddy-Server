using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Web__Api
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Password { get; set; }

        [Required]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Role { get; set; }  //admin,user,rider

        // public byte[] Pic { get; set; }

        public string CreatedAt { get; set; } = DateTime.Now.ToString();

        public string UpdatedAt { get; set; } = DateTime.Now.ToString();
        // Navigation properties
        public ICollection<Address>? Addresses { get; set; }
        public ICollection<PickUp>? Pickups { get; set; }

    }
}
