using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    [cite_start]// Spor salonu hizmetlerini tanımlayan sınıf 
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } // Örn: Pilates, Fitness

        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; } // Ders süresi

        [Display(Name = "Ücret (TL)")]
        public decimal Price { get; set; } // Ders ücreti

        // Bu hizmeti veren antrenörlerin listesi
        public ICollection<Trainer> Trainers { get; set; }
    }
}