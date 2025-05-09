using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VID.Models;

namespace VID.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //public DbSet<Admin> Admin { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<NextOfKin> NextOfKin { get; set; }
        public DbSet<Applicant> Applicant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<Person>()
            .ToTable("person");

            modelBuilder.Entity<Person>()
            .HasKey(p => p.IdentityId);

            modelBuilder.Entity<Person>()
            .HasKey(p => p.PersonId);

             modelBuilder.Entity<Person>()
            .Property(p => p.Email)
            .IsRequired();

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Person>()
            .Property(p => p.UserPassword)
            .IsRequired();

            base.OnModelCreating(modelBuilder);
    
            modelBuilder.Entity<Application>()
            .Property(e => e.DOB)
            .HasColumnType("timestamp with time zone");
            
            base.OnModelCreating(modelBuilder);


           var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
                v => v
            );

            modelBuilder.Entity<Application>()
                .Property(e => e.DOB)
                .HasConversion(dateTimeConverter);
             
            modelBuilder.Entity<Person>()
                .Property(p => p.PersonId)
                .HasIdentityOptions(1, 1, null, null, null, null);
        
                modelBuilder.Entity<Applicant>()
                    .Property(a => a.ApplicantId)
                    .HasIdentityOptions(1, 1, null, null, null, null);

                modelBuilder.Entity<NextOfKin>()
                    .Property(n => n.NextOfKinId)
                    .HasIdentityOptions(1, 1, null, null, null, null);

                modelBuilder.Entity<Application>()
                    .Property(ap => ap.ApplicationId)
                    .HasIdentityOptions(1, 1, null, null, null, null);

                modelBuilder.Entity<Status>()
                    .Property(s => s.StatusID)
                    .HasIdentityOptions(1, 1, null, null, null, null);

                modelBuilder.Entity<Document>()
                    .Property(d => d.DocumentId)
                    .HasIdentityOptions(1, 1, null, null, null, null);




        //     modelBuilder.Entity<Document>(entity =>
        //      {
        //         entity.Property(e => e.DocumentId)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");

            
        //         entity.Property(e => e.IdentityId)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");
     
        //     });

        //      modelBuilder.Entity<Person>(entity =>
        //      {
        //         entity.Property(e => e.IdentityId)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");

        //     });
            
        //     modelBuilder.Entity<NextOfKin>(entity =>
        //      {
        //         entity.Property(e => e.NextOfKinId)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");

            
        //         entity.Property(e => e.Identity_id)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");


        //         entity.Property(e => e.UserId)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");

                
        //         entity.Property(e => e.DocumentId)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");
        //     });

        //     modelBuilder.Entity<User>(entity =>
        //      {
        //         entity.Property(e => e.UserId)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");

            
        //         entity.Property(e => e.Identity_id)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");


        //         entity.Property(e => e.DocumentId)
        //             .HasColumnType("uuid")
        //             .HasDefaultValueSql("gen_random_uuid()");

        //     });
        // }
    }
    }
}
