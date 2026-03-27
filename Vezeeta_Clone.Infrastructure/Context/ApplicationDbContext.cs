using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vezeeta_Clone.Data.Entities;
namespace Vezeeta_Clone.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IEncryptionProvider _encryptionProvider;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _encryptionProvider = new GenerateEncryptionProvider("1ec364573a27a4d8");
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserToken> Tokens { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EPrescription> EPrescriptions { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<DoctorAvailabilitySlot> DoctorAvailabilitySlots { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<SubSpecialization> SubSpecializations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseEncryption(_encryptionProvider);
            builder.Entity<Doctor>(entity =>
            {

                entity.HasOne(d => d.ApplicationUser)
                    .WithOne()
                    .HasForeignKey<Doctor>(d => d.AppUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Clinic)
                       .WithOne(c => c.Doctor)
                       .HasForeignKey<Clinic>(c => c.DoctorId)
                       .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(d => d.Specialization)
                    .WithMany(s => s.Doctors)
                    .HasForeignKey(d => d.SpecializationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.SubSpecializations)
                    .WithMany(s => s.Doctors)
                    .UsingEntity(j => j.ToTable("DoctorSubSpecializations"));

            });



            builder.Entity<Patient>(entity =>
            {

                entity.HasOne(p => p.ApplicationUser)
                    .WithOne()
                    .HasForeignKey<Patient>(p => p.AppUserID)
                    .OnDelete(DeleteBehavior.Restrict);

            });


            builder.Entity<DoctorPatient>(entity =>
            {

                entity.HasKey(e => e.ID);

                entity.HasIndex(e => new { e.DoctorId, e.PatientId })
                      .IsUnique();

                entity.HasOne(dp => dp.Doctor)
                     .WithMany(d => d.DoctorPatients)
                     .HasForeignKey(dp => dp.DoctorId)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(db => db.Patient)
                       .WithMany(db => db.DoctorPatients)
                       .HasForeignKey(db => db.PatientId)
                       .OnDelete(DeleteBehavior.Restrict);

            });



            builder.Entity<Notification>(entity =>
            {
                entity.HasKey(not => not.ID);

                entity.HasOne(not => not.AppUser)
                        .WithMany(not => not.Notifications)
                        .HasForeignKey(not => not.UserId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<DoctorAvailabilitySlot>(entity =>
            {

                entity.HasKey(das => das.ID);
                entity.HasOne(das => das.Availability)
                       .WithMany(das => das.AvailableSlots)
                       .HasForeignKey(das => das.DoctorAvailabilityId)
                       .OnDelete(DeleteBehavior.Restrict);

            });
            //one to one between Appointment and AvailableSlot
            builder.Entity<Appointment>(entity =>
            {
                //entity.HasKey(a => a.ID);
                //entity.HasOne(a => a.AvailableSlot)
                //       .WithOne(a => a.Appointment)
                //       .HasForeignKey<Appointment>(a => a.SlotId)
                //       .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(a => a.AvailableSlot)
                     .WithMany(s => s.Appointments)
                     .HasForeignKey(a => a.SlotId)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Patient)
                     .WithMany(p => p.Appointments)
                     .HasForeignKey(a => a.PatientId)
                     .OnDelete(DeleteBehavior.Restrict);


            });

            foreach (var relationship in builder.Model.GetEntityTypes()
             .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            //soft delete for all entities inherits from baseclasss
            foreach (var entityType in builder.Model.GetEntityTypes()
                .Where(et => typeof(BaseEntity).IsAssignableFrom(et.ClrType)))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "entity");
                var property = Expression.Property(parameter, "IsDeleted");
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                builder.Entity(entityType.ClrType)
                    .HasQueryFilter(lambda);
            }


            base.OnModelCreating(builder);
        }
    }
}
