using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionVisualizer.Shared
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<ProjectAccess> ProjectAccesses { get; set; }
        public DbSet<UserCustomization> UserCustomizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Images)
                .WithOne(pi => pi.Project)
                .HasForeignKey(pi => pi.ProjectId);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.SharedAccess)
                .WithOne(pa => pa.Project)
                .HasForeignKey(pa => pa.ProjectId);
        }
    }
}
