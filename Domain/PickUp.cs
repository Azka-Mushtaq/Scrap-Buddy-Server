using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Pickup
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int CustomerId { get; set; }

        [Required]
        [ForeignKey("Address")]
        public int AddressId { get; set; }

        [ForeignKey("User")]
        public int RiderId { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public TimeSpan PickupTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Status { get; set; } = "pending";

        [Required]
        public string? TotalWeight { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }=DateTime.Now;

    }
}
