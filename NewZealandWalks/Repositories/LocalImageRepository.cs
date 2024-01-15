using NewZealandWalks.Data;
using NewZealandWalks.Models.Domain;

namespace NewZealandWalks.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment
            ,IHttpContextAccessor httpContextAccessor
            ,ApplicationDbContext db)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", 
                $"{image.FileName}{image.FileExtension}");

            //upload image to local path
            using var stream = new FileStream(localFilePath,FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;
            
            //Add images to ImageTable
            await _db.Images.AddAsync(image);
            await _db.SaveChangesAsync();

            return image;
        }
    }
}
