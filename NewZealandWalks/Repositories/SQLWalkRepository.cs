using NewZealandWalks.Data;
using NewZealandWalks.Models.Domain;

namespace NewZealandWalks.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly ApplicationDbContext _db;

        public SQLWalkRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _db.Walks.AddAsync(walk);
            await _db.SaveChangesAsync();
            return walk;
        }
    }
}
