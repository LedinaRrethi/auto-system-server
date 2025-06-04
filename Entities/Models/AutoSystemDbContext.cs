using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Entities.Models
{
    public partial class AutoSystemDbContext : IdentityDbContext<Auto_Users>
    {
        public AutoSystemDbContext(DbContextOptions<AutoSystemDbContext> options)
            : base(options) { }

        // Add custom DbSets later if needed

        public DbSet<Auto_Users> Auto_Users { get; set; }
        public DbSet<Auto_Vehicles> Auto_Vehicles { get; set; }
        public DbSet<Auto_VehicleChangeRequests> Auto_VehicleChangeRequests { get; set; }
        public DbSet<Auto_FineRecipients> Auto_FineRecipients { get; set; }
        public DbSet<Auto_Fines> Auto_Fines { get; set; }
        public DbSet<Auto_Directorates> Auto_Directorates { get; set; }
        public DbSet<Auto_InspectionRequests> Auto_InspectionRequests { get; set; }
        public DbSet<Auto_Inspections> Auto_Inspections { get; set; }
        public DbSet<Auto_InspectionDocs> Auto_InspectionDocs { get; set; }
        public DbSet<AutoNotification> Auto_Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Auto_Vehicles>()
                .HasIndex(v => v.PlateNumber)
                .IsUnique();

            modelBuilder.Entity<Auto_Vehicles>()
                .HasIndex(v => v.ChassisNumber)
                .IsUnique();

            modelBuilder.Entity<Auto_Vehicles>()
                .Property(v => v.Status)
                .HasConversion<byte>(); // ruhet si byte në db


            modelBuilder.Entity<Auto_VehicleChangeRequests>()
                .Property(e => e.RequestType)
                .HasConversion<byte>();

            modelBuilder.Entity<Auto_VehicleChangeRequests>()
                .Property(e => e.Status)
                .HasConversion<byte>();

            modelBuilder.Entity<Auto_Fines>()
                .Property(f => f.FineAmount)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Auto_FineRecipients>()
                .HasIndex(f => f.PlateNumber);

            modelBuilder.Entity<Auto_FineRecipients>()
                .HasIndex(f => f.PersonalId)
                .IsUnique()
                .HasFilter("[PersonalId] IS NOT NULL"); // për të lejuar nullable

            modelBuilder.Entity<Auto_Directorates>()
                .HasIndex(d => d.DirectoryName)
                .IsUnique();

            modelBuilder.Entity<Auto_InspectionRequests>()
                .Property(e => e.Status)
                .HasConversion<byte>();

            modelBuilder.Entity<Auto_InspectionDocs>()
                .Property(d => d.FileBase64)
                .HasMaxLength(7_000_000); // Siguron që edhe në DB të jetë e kufizuar

            modelBuilder.Entity<Auto_Inspections>()
                .Property(i => i.IsPassed)
                .HasDefaultValue(false);

            modelBuilder.Entity<Auto_Users>()
                .HasOne(u => u.Directorate)
                .WithMany(d => d.Specialists)
                .HasForeignKey(u => u.IDFK_Directory)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_Users>()
    .HasOne(u => u.Creator)
    .WithMany()
    .HasForeignKey(u => u.CreatedBy)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_Users>()
                .HasOne(u => u.Modifier)
                .WithMany()
                .HasForeignKey(u => u.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_Directorates>()
                .HasOne(d => d.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_Directorates>()
                .HasOne(d => d.ModifiedByUser)
                .WithMany()
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_FineRecipients>()
    .HasOne(f => f.CreatedByUser)
    .WithMany()
    .HasForeignKey(f => f.CreatedBy)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_FineRecipients>()
                .HasOne(f => f.ModifiedByUser)
                .WithMany()
                .HasForeignKey(f => f.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
  }



