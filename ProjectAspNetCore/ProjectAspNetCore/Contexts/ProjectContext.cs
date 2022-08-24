using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectAspNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Contexts
{
    public class ProjectContext : IdentityDbContext<AppUser, AppRole, int>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=LAPTOP-UI4AV1F3; database=FaturaTahsilatDb; integrated security=true;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FaturaBilgi>().HasMany(I => I.FaturaBilgiKategoriler).WithOne
                (I => I.FaturaBilgi).HasForeignKey(I => I.FaturaId);

            modelBuilder.Entity<FaturaBilgi>().HasOne(x => x.AppUserCustomer).WithMany(x => x.CustomerFatura).HasForeignKey(x => x.CustomerId).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.ClientSetNull); ;
            modelBuilder.Entity<FaturaBilgi>().HasOne(x => x.AppUserCompany).WithMany(x => x.CompanyFatura).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<FaturaKategori>().HasMany(I => I.FaturaBilgiKategori).WithOne
                (I => I.FaturaKategori).HasForeignKey(I => I.KategoriId);

            modelBuilder.Entity<FaturaBilgiKategori>().HasIndex(I => new
            {
                I.KategoriId,
                I.FaturaId
            }).IsUnique();
            base.OnModelCreating(modelBuilder);

        }


        public DbSet<FaturaBilgiKategori> FaturaBilgiKategoriler { get; set; }
        public DbSet<FaturaBilgi> FaturaBilgiler { get; set; }
        public DbSet<FaturaKategori> FaturaKategori { get; set; }
    }
}
