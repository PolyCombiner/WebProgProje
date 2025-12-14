using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        // HATA ÇÖZÜMÜ BURADA:
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Identity ayarları için zorunlu

            // Randevu - Eğitmen ilişkisinde zincirleme silmeyi kapatıyoruz
            builder.Entity<Appointment>()
                .HasOne(a => a.Trainer)
                .WithMany()
                .HasForeignKey(a => a.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Randevu - Hizmet ilişkisinde zincirleme silmeyi kapatıyoruz
            builder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany() // Service sınıfında Trainers listesi var ama Appointment listesi yoksa WithMany() boş kalabilir
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}