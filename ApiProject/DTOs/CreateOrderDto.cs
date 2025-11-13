using System.ComponentModel.DataAnnotations;

namespace ApiProject.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Shipping address must be between 10 and 500 characters")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
