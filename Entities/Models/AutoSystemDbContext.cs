using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    public partial class AutoSystemDbContext : IdentityDbContext<Auto_Users>
    {
        public AutoSystemDbContext(DbContextOptions<AutoSystemDbContext> options)
            : base(options) { }

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

            // VEHICLES
            modelBuilder.Entity<Auto_Vehicles>()
                .HasIndex(v => v.PlateNumber)
                .IsUnique();

            modelBuilder.Entity<Auto_Vehicles>()
                .HasIndex(v => v.ChassisNumber)
                .IsUnique();

            modelBuilder.Entity<Auto_Vehicles>()
                .Property(v => v.Status)
                .HasConversion<byte>();

            // VEHICLE CHANGE REQUESTS
            modelBuilder.Entity<Auto_VehicleChangeRequests>()
                .Property(e => e.RequestType)
                .HasConversion<byte>();

            modelBuilder.Entity<Auto_VehicleChangeRequests>()
                .Property(e => e.Status)
                .HasConversion<byte>();

            // FINES
            modelBuilder.Entity<Auto_Fines>()
                .Property(f => f.FineAmount)
                .HasColumnType("decimal(10,2)");

            // FINE RECIPIENTS
            modelBuilder.Entity<Auto_FineRecipients>()
                .HasIndex(f => f.PlateNumber);

            modelBuilder.Entity<Auto_FineRecipients>()
                .HasIndex(f => f.PersonalId)
                .IsUnique()
                .HasFilter("[PersonalId] IS NOT NULL");  // për të lejuar nullable

            // DIRECTORATES
            modelBuilder.Entity<Auto_Directorates>()
                .HasIndex(d => d.DirectoryName)
                .IsUnique();

            // INSPECTION REQUESTS
            modelBuilder.Entity<Auto_InspectionRequests>()
                .Property(e => e.Status)
                .HasConversion<byte>();

            // INSPECTION DOCS
            modelBuilder.Entity<Auto_InspectionDocs>()
                .Property(d => d.FileBase64)
                .HasMaxLength(7_000_000); // Siguron që edhe në DB të jetë e kufizuar

            // INSPECTIONS
            modelBuilder.Entity<Auto_Inspections>()
                .Property(i => i.IsPassed)
                .HasDefaultValue(false);

            // =========================
            // RELATIONSHIPS SETUP
            // =========================

            // Auto_Users
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

            // Auto_Directorates
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

            // Auto_FineRecipients
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

            // Auto_Vehicles
            modelBuilder.Entity<Auto_Vehicles>()
                .HasOne(v => v.Owner)
                .WithMany()
                .HasForeignKey(v => v.IDFK_Owner)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_Vehicles>()
                .HasOne(v => v.CreatedByUser)
                .WithMany()
                .HasForeignKey(v => v.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_Vehicles>()
                .HasOne(v => v.ModifiedByUser)
                .WithMany()
                .HasForeignKey(v => v.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Auto_VehicleChangeRequests
            modelBuilder.Entity<Auto_VehicleChangeRequests>()
                .HasOne(r => r.Requester)
                .WithMany()
                .HasForeignKey(r => r.IDFK_Requester)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_VehicleChangeRequests>()
                .HasOne(r => r.CreatedByUser)
                .WithMany()
                .HasForeignKey(r => r.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_VehicleChangeRequests>()
                .HasOne(r => r.ModifiedByUser)
                .WithMany()
                .HasForeignKey(r => r.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_VehicleChangeRequests>()
                .HasOne(r => r.Vehicle)
                .WithMany()
                .HasForeignKey(r => r.IDFK_Vehicle)
                .OnDelete(DeleteBehavior.Restrict);

            // Auto_Inspections
            modelBuilder.Entity<Auto_Inspections>()
                .HasOne(i => i.Request)
                .WithMany()
                .HasForeignKey(i => i.IDFK_InspectionRequest)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_Inspections>()
                .HasOne(i => i.Specialist)
                .WithMany()
                .HasForeignKey(i => i.IDFK_Specialist)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_Inspections>()
                .HasOne(i => i.CreatedByUser)
                .WithMany()
                .HasForeignKey(i => i.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auto_Inspections>()
                .HasOne(i => i.ModifiedByUser)
                .WithMany()
                .HasForeignKey(i => i.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}



