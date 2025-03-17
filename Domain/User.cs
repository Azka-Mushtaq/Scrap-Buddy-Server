using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Domain
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
        public string? Password { get; set; } = "not entered";

        [Required]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Role { get; set; }  //admin,user,scrap Picker

       // public byte[] Pic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<Address>? Addresses { get; set; }
        public ICollection<Pickup>? Pickups { get; set; }

    }
}
