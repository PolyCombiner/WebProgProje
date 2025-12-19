using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FitnessCenterApp.Controllers
{
    
    [Authorize] // Randevu işlemleri için giriş yapılmış olması şarttır [cite: 53]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Veritabanı ve Kullanıcı Yönetimi servislerini içeri alıyoruz
        public AppointmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            
            var query = _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(a => a.UserId == currentUserId);
            }

            return View(await query.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName");
            return View();
        }

        // POST: Appointments/Create
        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppointmentDate,TrainerId,ServiceId,IsConfirmed")] Appointment appointment)
        {
            // 1. Giriş yapan kullanıcının ID'sini otomatik olarak atıyoruz
            appointment.UserId = _userManager.GetUserId(User);

            // 2. UserId formdan gelmediği için doğrulama hatasını manuel olarak temizliyoruz
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                // 3. Seçilen hizmetin süresini veritabanından çekiyoruz
                var service = await _context.Services.FindAsync(appointment.ServiceId);
                if (service == null) return NotFound();

                // 4. Yeni randevunun başlangıç ve bitiş zamanını hesaplıyoruz
                var newStart = appointment.AppointmentDate;
                var newEnd = newStart.AddMinutes(service.Duration);

                // 5. Çakışma Kontrolü (LINQ): Antrenör bu saatte meşgul mü?
                // Mantık: (Mevcut Başlangıç < Yeni Bitiş) VE (Yeni Başlangıç < Mevcut Bitiş)
                var isBusy = await _context.Appointments
                    .AnyAsync(a => a.TrainerId == appointment.TrainerId &&
                                   a.AppointmentDate < newEnd &&
                                   newStart < a.AppointmentDate.AddMinutes(a.Service.Duration));

                if (isBusy)
                {
                    // Eğer meşgulse hata mesajı ekle ve sayfayı geri döndür
                    ModelState.AddModelError("", "Seçilen antrenör bu saat aralığında başka bir randevusu olduğu için müsait değildir.");
                    ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
                    ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
                    return View(appointment);
                }

                // 6. Her şey yolundaysa kaydet
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Model geçersizse dropdown listelerini tekrar doldur
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,AppointmentDate,TrainerId,ServiceId,IsConfirmed")] Appointment appointment)
        {
            if (id != appointment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}