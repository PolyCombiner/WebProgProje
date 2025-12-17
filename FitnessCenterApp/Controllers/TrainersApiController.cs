using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TrainersApi
        // Bu metod tüm antrenörleri JSON olarak döndürür (LINQ Kullanımı: ToListAsync)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainers()
        {
            return await _context.Trainers
                                 .Include(t => t.Service) // İlişkili veriyi de getir
                                 .ToListAsync();
        }

        // GET: api/TrainersApi/ByService/1
        // Bu metod belirli bir hizmeti veren hocaları filtreler (LINQ Where Kullanımı)
        [HttpGet("ByService/{serviceId}")]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainersByService(int serviceId)
        {
            var trainers = await _context.Trainers
                                         .Where(t => t.ServiceId == serviceId) // LINQ Filtreleme
                                         .Include(t => t.Service)
                                         .ToListAsync();

            if (trainers == null || trainers.Count == 0)
            {
                return NotFound("Bu dersi veren hoca bulunamadı.");
            }

            return trainers;
        }
    }
}