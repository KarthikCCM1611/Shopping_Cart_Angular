using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Image { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
    }
}
