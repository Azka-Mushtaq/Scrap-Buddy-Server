using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int RiderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? VehicleType { get; set; }

        [Required]
        [MaxLength(20)]
        public string? VehicleNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
