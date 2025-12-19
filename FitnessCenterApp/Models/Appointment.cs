using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public string? UserId { get; set; } // Nullable olmalı

        [Required(ErrorMessage = "Lütfen bir tarih seçiniz.")]
        [Display(Name = "Randevu Tarihi ve Saati")]
        public DateTime AppointmentDate { get; set; }

        public int TrainerId { get; set; }

        // KRİTİK NOKTA: Bunların yanına ? eklemeliyiz 
        // Yoksa sistem bunları formdan "zorunlu" bekler ve hata verir.
        public virtual Trainer? Trainer { get; set; }

        public int ServiceId { get; set; }
        public virtual Service? Service { get; set; }

        public bool IsConfirmed { get; set; } = false;
    }
}