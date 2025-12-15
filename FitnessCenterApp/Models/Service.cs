using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; }

        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; }

        [Display(Name = "Ücret (TL)")]
        public decimal Price { get; set; }

        // Bu hizmeti veren antrenörlerin listesi
        public ICollection<Trainer>? Trainers { get; set; }
    }
}