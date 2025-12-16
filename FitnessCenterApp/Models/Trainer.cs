using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Display(Name = "Uzmanlık Alanı")]
        public string? Specialization { get; set; } // Soru işareti eklendi

        public int ServiceId { get; set; }

        // ÖNEMLİ OLAN BURASI:
        public Service? Service { get; set; } // Soru işareti eklendi

        public string? Availability { get; set; } // Soru işareti eklendi
    }
}