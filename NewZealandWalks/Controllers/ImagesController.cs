using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalks.Models.Domain;
using NewZealandWalks.Models.DTOs;
using NewZealandWalks.Repositories;

namespace NewZealandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        //POST:/api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO request) 
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid) 
            {
                //Dto to Domain Model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };
                //User repository to upload image
                await _imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(ImageUploadRequestDTO request) 
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName))) 
            {
                ModelState.AddModelError("file", "Unsupported file Extension");
            }
            if (request.File.Length > 10485760) 
            {
                ModelState.AddModelError("file", "File Size Should be less than 10Mb");
            }
        
        }
    }

}
 