using Microsoft.EntityFrameworkCore;
using NewZealandWalks.Data;
using NewZealandWalks.Models.Domain;

namespace NewZealandWalks.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly ApplicationDbContext _db;

        public SQLRegionRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await _db.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
