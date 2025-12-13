using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    [cite_start]// Antrenörleri tanımlayan sınıf [cite: 15, 16]
    public class Trainer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Display(Name = "Uzmanlık Alanı")]
        public string Specialization { get; set; } // Örn: Kilo Verme, Kas Yapma

        // Antrenörün verebildiği hizmet (Hangi hizmete bağlı olduğu)
        // Basitlik olması için bir antrenör şimdilik bir ana hizmet grubunda olsun diyelim
        // Veya çoka-çok ilişki yerine basit ilişki kuralım:
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        [cite_start]// Müsaitlik saatleri (Basit metin olarak tutalım şimdilik, örn: "09:00-17:00") [cite: 17]
        public string Availability { get; set; }
    }
}