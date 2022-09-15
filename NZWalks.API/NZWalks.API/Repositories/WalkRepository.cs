using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        //----------------------------------------------------------------------------------------------------------------
        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDBContext.Walks.AddAsync(walk);
            await nZWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk = await nZWalksDBContext.Walks.FindAsync(id);
            if (walk == null)
            {
                return null;
            };
            nZWalksDBContext.Walks.Remove(walk);
            await nZWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            //Originale su REgion dovrebbe essere così: return await nZWalksDBContext.Walks.ToListAsync();
            //la riga seguente restituisce tutto il contenuto di ciascuna Walk ma non i campi relazionati (WalkDifficulty e Region)
            //return await nZWalksDBContext.Walks.ToListAsync();
            //in questo modo invece restituisce tutto il contenuto delle tabelle relazionate tramite l'uso delle Navigation Property
            //definite sia nel Domain che nel DTO model
            return await nZWalksDBContext.Walks
                .Include(x => x.Region )
                .Include(x => x.WalkDifficulty)
                .ToListAsync();

        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDBContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nZWalksDBContext.Walks.FindAsync(id);
            if (existingWalk == null)
            {
                return null;
            };
            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            await nZWalksDBContext.SaveChangesAsync();
            return existingWalk;

        }
    }
} 