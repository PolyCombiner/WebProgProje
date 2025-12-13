using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    [cite_start]// Randevu detaylarını saklayan sınıf [cite: 20]
    public class Appointment
    {
        public int Id { get; set; }

        // Randevuyu alan üyenin ID'si (IdentityUser kullanacağız ileride)
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Randevu Tarihi ve Saati")]
        public DateTime AppointmentDate { get; set; }

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } // Hangi hoca?

        public int ServiceId { get; set; }
        public Service Service { get; set; } // Hangi ders?

        public bool IsConfirmed { get; set; } = false; // Onay durumu [cite: 21]
    }
}