using Microsoft.EntityFrameworkCore;
using Poznamky.Models;

namespace Poznamky.Data
{
    public class PoznamkyData : DbContext
    {
        public DbSet<UzivatelModel> Uzivatele { get; set; }
        public DbSet<PoznamkaModel> Poznamky { get; set; }

        public PoznamkyData(DbContextOptions<PoznamkyData> options) : base(options) { }
    }

    
}
