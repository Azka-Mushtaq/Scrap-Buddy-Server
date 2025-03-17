using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class PickupScrapItem
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Pickup")]
        public int PickupId { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("ScrapItem")]
        public int ScrapItemId { get; set; }

        [Required]
        public decimal QuantityKg { get; set; }

        // Navigation properties
        public ScrapItem? ScrapItem { get; set; }
    }
}
