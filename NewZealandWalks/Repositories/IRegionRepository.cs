using NewZealandWalks.Models.Domain;

namespace NewZealandWalks.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();


    }
}
