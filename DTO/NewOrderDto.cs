using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class NewOrderDto
    {
        [Required]
        public string StartPoint { get; set; }

        [Required]
        public string EndPoint { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
