using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        // Randevuyu alan üyenin ID'si
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Randevu Tarihi ve Saati")]
        public DateTime AppointmentDate { get; set; }

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public bool IsConfirmed { get; set; } = false;
    }
}