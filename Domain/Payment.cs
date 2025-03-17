using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pickup")]
        public int PickupId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Pickup? Pickup { get; set; }
    }
}
