using Microsoft.AspNetCore.Identity;
using FitnessCenterApp.Constants; // Birazdan oluşturacağız, hata verirse bekle

namespace FitnessCenterApp.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Kullanıcı yönetimi ve Rol yönetimi servislerini çağırıyoruz
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. Roller Var mı? Yoksa oluştur.
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            await roleManager.CreateAsync(new IdentityRole(Roles.Member));

            // 2. Admin Kullanıcısı Var mı?
            // Hoca şifreyi "sau" istemiş, maili de öğrenci numarası formatında.
            var adminEmail = "b211210062@sakarya.edu.tr"; // BURAYI KENDİ NUMARANLA DEĞİŞTİR
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                };

                // Kullanıcıyı oluştur ve şifresini 'sau' yap
                var result = await userManager.CreateAsync(newAdmin, "sau");

                if (result.Succeeded)
                {
                    // Kullanıcıya Admin rolünü ver
                    await userManager.AddToRoleAsync(newAdmin, Roles.Admin);
                }
            }
        }
    }
}