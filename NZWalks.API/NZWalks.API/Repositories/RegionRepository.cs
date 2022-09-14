using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository

    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public RegionRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id= Guid.NewGuid();
            await nZWalksDBContext.Regions.AddAsync(region);
            await nZWalksDBContext.SaveChangesAsync(); 
            return region;  
        }
        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id==id);
            if (region == null)
            {
                return null;
            }
            nZWalksDBContext.Regions.Remove(region);
            await nZWalksDBContext.SaveChangesAsync(); //per essere sicuri che SaveChanges ha avuto successo bisognerebbe fare un get del record per vedere se l'ha eliminato
            return region;
        }
        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            //aggiorno tutti i dati tranne l'Id che ovviamente non devo modificare 
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;
            //nZWalksDBContext.Regions.Update(existingRegion);
            await nZWalksDBContext.SaveChangesAsync(); //per essere sicuri che SaveChanges ha avuto successo bisognerebbe fare un get del record per vedere se l'ha aggiornato
            return existingRegion;

        }


        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDBContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
