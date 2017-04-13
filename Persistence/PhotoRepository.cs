using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;

namespace vega.Persistence
{
  public class PhotoRepository : IPhotoRepository
  {
    private readonly VegaDbContext context;
    public PhotoRepository(VegaDbContext context)
    {
      this.context = context;
    }
    public async Task<IEnumerable<Photo>> GetPhotos(int vehicleId)
    {
      return await context.Photos
        .Where(p => p.VehicleId == vehicleId)
        .ToListAsync();
    }
  }
}