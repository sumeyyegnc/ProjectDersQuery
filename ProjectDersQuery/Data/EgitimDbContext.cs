using Microsoft.EntityFrameworkCore;
using ProjectDersQuery.Models;

namespace ProjectDersQuery.Data
{
    public class EgitimDbContext:DbContext
    {

        public EgitimDbContext(DbContextOptions<EgitimDbContext> options)
            : base(options)
        {

        }

        public DbSet<Ogrenci> Ogrenciler { get; set; }
        public DbSet<Egitmen> Egitmenler { get; set; }
        public DbSet<Kurs> Kurslar { get; set; }


    }
}
