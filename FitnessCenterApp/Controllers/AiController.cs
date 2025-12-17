using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FitnessCenterApp.Controllers
{
    [Authorize] // Sadece üyeler girebilsin
    public class AiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GeneratePlan(int weight, int height, string goal)
        {
            // Basit bir Yapay Zeka Mantığı (Simülasyon)
            string advice = "";
            string plan = "";

            // Vücut Kitle İndeksi (BMI) Hesabı
            double bmi = weight / ((height / 100.0) * (height / 100.0));

            if (goal == "zayiflama")
            {
                advice = $"Vücut kitle indeksiniz {bmi:F1}. Sizin için yağ yakımına odaklı, yüksek tempolu bir program hazırladım.";
                plan = "Öneri: Haftada 3 gün Kardiyo (Koşu/Bisiklet) + 2 gün Pilates.";
            }
            else if (goal == "kas")
            {
                advice = $"Vücut kitle indeksiniz {bmi:F1}. Kas kütlenizi artırmak için protein ağırlıklı beslenme ve ağırlık antrenmanı şart.";
                plan = "Öneri: Haftada 4 gün Fitness (Ağırlık) + 1 gün Yüzme.";
            }
            else
            {
                advice = $"Vücut kitle indeksiniz {bmi:F1}. Formunuzu korumak ve esneklik kazanmak için dengeli bir program uygun.";
                plan = "Öneri: Haftada 3 gün Yoga + Hafif yürüyüşler.";
            }

            // Sonuçları sayfaya geri gönder
            ViewBag.Advice = advice;
            ViewBag.Plan = plan;
            ViewBag.ShowResult = true;

            return View("Index");
        }
    }
}