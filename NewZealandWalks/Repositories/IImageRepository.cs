using NewZealandWalks.Models.Domain;

namespace NewZealandWalks.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
