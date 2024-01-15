using System.ComponentModel.DataAnnotations;

namespace NewZealandWalks.Models.DTOs
{
    public class ImageUploadRequestDTO
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }

    }
}
