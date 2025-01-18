using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _6.NovaPoshta.Constants;
using _6.NovaPoshta.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace _6.NovaPoshta.Data
{
    public class BombaDbContext : DbContext
    {
        public DbSet<AreaEntity> Areas { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(AppDatabase.ConnectionString);
        }

    }
}
