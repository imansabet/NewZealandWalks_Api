using System.ComponentModel.DataAnnotations;

namespace NewZealandWalks.Models.DTOs
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MaxLength(3,ErrorMessage = "Code has to be a minimum of 3 Char")]
        [MinLength(3, ErrorMessage = "Code has to be a maximum of 3 Char")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a minimum of 100 Char")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
