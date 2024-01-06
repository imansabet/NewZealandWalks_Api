using NewZealandWalks.Models.Domain;

namespace NewZealandWalks.Repositories
{
    public class InMemmoryRegionRepository : IRegionRepository
    {
        public async Task<List<Region>> GetAllAsync()
        {
            return new List<Region>
            {
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "Karaj",
                    Name = "Karaaaaj",

                }
                
            };
        }
    }
} 
