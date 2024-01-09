using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null)
        { 
            var walks = _db.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false) 
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase)) 
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            return await walks.ToListAsync();

        }
        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _db.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _db.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null) 
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;

            await _db.SaveChangesAsync();
            return existingWalk;
        }
        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await _db.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null) 
            {
                return null; 
            }
            _db.Walks.Remove(existingWalk);
            await _db.SaveChangesAsync();
            return existingWalk;
        }

    }
}
 