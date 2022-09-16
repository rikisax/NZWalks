using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkDifficultyRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        //----------------------------------------------------------------------------------------------------------------
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDBContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDBContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var walkDifficulty = await nZWalksDBContext.WalkDifficulty.FindAsync(id);
            if (walkDifficulty == null)
            {
                return null;
            };
            nZWalksDBContext.WalkDifficulty.Remove(walkDifficulty);
            await nZWalksDBContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDBContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDBContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await nZWalksDBContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDifficulty == null)
            {
                return null;
            };
            existingWalkDifficulty.Code = walkDifficulty.Code;
            await nZWalksDBContext.SaveChangesAsync();
            return existingWalkDifficulty;

        }
    }
} 