using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web__Api
{
    public class PickUp
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
        public string? PickupDate { get; set; }

        [Required]
        public string? PickupTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Status { get; set; } = "pending";

        [Required]
        public string? TotalWeight { get; set; }

        public string? CreatedAt { get; set; } = DateTime.Now.ToString();

        public string? UpdatedAt { get; set; }=DateTime.Now.ToString();

    }
}
