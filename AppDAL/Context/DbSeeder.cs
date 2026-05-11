using AppBL.Mapper;
using AppDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppDAL.Context
{
    public class DbSeeder
    {
        public static async Task SeedAsync(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            HotelDbContext context)
        {
            await context.Database.MigrateAsync();

            // =========================
            // 🔐 ROLES
            // =========================
            string[] roles = { Roles.Manager, Roles.Employee, Roles.Partner };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new AppRole { Name = role });
            }

            // =========================
            // 👑 ADMIN
            // =========================
            var adminEmail = "babnha52@gmail.com".ToLower();
            var def = "def@admin.com".ToLower();
            var admin = await userManager.FindByEmailAsync(adminEmail);
            var defAdmin = await userManager.FindByEmailAsync(def);
            if (admin == null)
            {
                admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "احمد عبابنه",
                    EmailConfirmed = true,
                    MustChangePassword = true,
                    IsActive=true
                };

                await userManager.CreateAsync(admin, "Admin@12345");
            }
            if (defAdmin == null)
            {
                defAdmin = new AppUser
                {
                    UserName = def,
                    Email = def,
                    FullName = "ادمن احطياتي",
                    EmailConfirmed = true,
                    MustChangePassword = true,
                    IsActive = true
                };

                await userManager.CreateAsync(defAdmin, "Admin@12345");
            }
            await userManager.AddToRoleAsync(defAdmin, Roles.Manager);

            // =========================
            // 👤 PARTNERS
            // =========================
            var partner1 = await EnsureUser(userManager, "partner1@test.com", "شريك الغزال الأول");
            var partner2 = await EnsureUser(userManager, "Mohammad.ayyoub@gmail.com", "محمد ايوب");

            await userManager.AddToRoleAsync(partner1, Roles.Partner);
            await userManager.AddToRoleAsync(partner2, Roles.Partner);

            // =========================
            // 💰 PRICING (18)
            // =========================
            if (!await context.Pricings.AnyAsync())
            {
                context.Pricings.AddRange(

                    // 🏡 NORMAL
                    new Pricing { ChaletType = ChaletType.Normal, Period = BookingPeriod.Morning, DayType = DayType.Weekday, Price = 60 },
                    new Pricing { ChaletType = ChaletType.Normal, Period = BookingPeriod.Evening, DayType = DayType.Weekday, Price = 70 },
                    new Pricing { ChaletType = ChaletType.Normal, Period = BookingPeriod.Full, DayType = DayType.Weekday, Price = 100 },

                    new Pricing { ChaletType = ChaletType.Normal, Period = BookingPeriod.Morning, DayType = DayType.Weekend, Price = 72 },
                    new Pricing { ChaletType = ChaletType.Normal, Period = BookingPeriod.Evening, DayType = DayType.Weekend, Price = 84 },
                    new Pricing { ChaletType = ChaletType.Normal, Period = BookingPeriod.Full, DayType = DayType.Weekend, Price = 120 },

                    new Pricing { ChaletType = ChaletType.Normal, Period = BookingPeriod.Morning, DayType = DayType.Holiday, Price = 102 },
                    new Pricing { ChaletType = ChaletType.Normal, Period = BookingPeriod.Evening, DayType = DayType.Holiday, Price = 119 },
                    new Pricing { ChaletType = ChaletType.Normal, Period = BookingPeriod.Full, DayType = DayType.Holiday, Price = 170 },

                    // 👑 ROYAL
                    new Pricing { ChaletType = ChaletType.Royal, Period = BookingPeriod.Morning, DayType = DayType.Weekday, Price = 130 },
                    new Pricing { ChaletType = ChaletType.Royal, Period = BookingPeriod.Evening, DayType = DayType.Weekday, Price = 100 },
                    new Pricing { ChaletType = ChaletType.Royal, Period = BookingPeriod.Full, DayType = DayType.Weekday, Price = 170 },

                    new Pricing { ChaletType = ChaletType.Royal, Period = BookingPeriod.Morning, DayType = DayType.Weekend, Price = 150 },
                    new Pricing { ChaletType = ChaletType.Royal, Period = BookingPeriod.Evening, DayType = DayType.Weekend, Price = 120 },
                    new Pricing { ChaletType = ChaletType.Royal, Period = BookingPeriod.Full, DayType = DayType.Weekend, Price = 190 },

                    new Pricing { ChaletType = ChaletType.Royal, Period = BookingPeriod.Morning, DayType = DayType.Holiday, Price = 190 },
                    new Pricing { ChaletType = ChaletType.Royal, Period = BookingPeriod.Evening, DayType = DayType.Holiday, Price = 160 },
                    new Pricing { ChaletType = ChaletType.Royal, Period = BookingPeriod.Full, DayType = DayType.Holiday, Price = 300 }
                );

                await context.SaveChangesAsync();
            }

            // =========================
            // 🍔 EXTRAS (كاملة)
            // =========================
            if (!await context.Extras.AnyAsync())
            {
                context.Extras.AddRange(

                    // 🔥 BBQ
                    new Extra { Name = "كيلو مشاوي مشكل", Price = 25, IsActive = true },
                    new Extra { Name = "كيلو كباب", Price = 25, IsActive = true },
                    new Extra { Name = "كيلو شقف", Price = 25, IsActive = true },
                    new Extra { Name = "كيلو شيش طاووق", Price = 15, IsActive = true },

                    // 🍗 Chicken
                    new Extra { Name = "دجاج مشوي", Price = 10, IsActive = true },
                    new Extra { Name = "صدر مندي", Price = 17, IsActive = true },
                    new Extra { Name = "صدر أوزي", Price = 17, IsActive = true },
                    new Extra { Name = "صدر كبسة", Price = 17, IsActive = true },
                    new Extra { Name = "صدر مسخن", Price = 17, IsActive = true },

                    // 🍳 Breakfast
                    new Extra { Name = "فطور بلدي (3 أشخاص)", Price = 10, IsActive = true },

                    // 🎉 Decoration
                    new Extra { Name = "زينة عادية", Price = 20, IsActive = true },
                    new Extra { Name = "زينة مميزة", Price = 30, IsActive = true },
                    new Extra { Name = "زينة فخمة", Price = 50, IsActive = true },
                    new Extra { Name = "زينة VIP", Price = 80, IsActive = true },

                    // 🎂 Add-ons
                    new Extra { Name = "قالب كيك", Price = 5, IsActive = true },
                    new Extra { Name = "بوكيه ورد", Price = 5, IsActive = true },
                    new Extra { Name = "بالونات", Price = 5, IsActive = true },
                    new Extra { Name = "كتابة على السرير", Price = 5, IsActive = true },
                    new Extra { Name = "عصير مع شموع", Price = 5, IsActive = true }
                );

                await context.SaveChangesAsync();
            }

            // =========================
            // 🏡 CHALETS
            // =========================
            if (!await context.Chalets.AnyAsync())
            {
                context.Chalets.AddRange(

                    new Chalet { Id = 101, Name = "كوخ 101", Type = ChaletType.Normal, Status = ChaletStatus.Available, HasFullDay = true },
                    new Chalet { Id = 102, Name = "كوخ 102", Type = ChaletType.Normal, Status = ChaletStatus.Available, HasFullDay = true },
                    new Chalet { Id = 103, Name = "كوخ 103", Type = ChaletType.Normal, Status = ChaletStatus.Available, HasFullDay = true },
                    new Chalet { Id = 104, Name = "كوخ 104", Type = ChaletType.Normal, Status = ChaletStatus.Available, HasFullDay = true },
                    new Chalet { Id = 105, Name = "كوخ رويال 105", Type = ChaletType.Royal, HasMorning = true, HasEvening = true },
                    new Chalet { Id = 106, Name = "كوخ رويال 106", Type = ChaletType.Royal, HasMorning = true, HasEvening = true },
                    new Chalet { Id = 107, Name = "كوخ رويال 107", Type = ChaletType.Royal, HasFullDay = true },
                    new Chalet { Id = 108, Name = "كوخ رويال 108", Type = ChaletType.Royal, HasFullDay = true }
                );

                await context.SaveChangesAsync();
            }
            // =========================
            // 🏡 Holydays
            // =========================
            if (!await context.Holidays.AnyAsync())
            {
                context.Holidays.AddRange(

                    // =========================
                    // 🇯🇴 National Holidays (ثابتة)
                    // =========================

                    new Holiday {  Name = "عيد الاستقلال", Date = new DateTime(DateTime.UtcNow.Year, 5, 25) },
                    new Holiday {  Name = "عيد الجلوس الملكي", Date = new DateTime(DateTime.UtcNow.Year, 6, 9) },
                    new Holiday {  Name = "عيد الثورة العربية الكبرى", Date = new DateTime(DateTime.UtcNow.Year, 6, 10) },
                    new Holiday {  Name = "عيد الجيش", Date = new DateTime(DateTime.UtcNow.Year, 6, 10) },

                    // ==================
                    // 🌙 Islamic s (متغيرة - تقديرية)
                    // ==================
                    //الحج / الأضحى
                    new Holiday {  Name = "يوم عرفة ", Date = new DateTime(DateTime.UtcNow.Year, 6, 16) },
                    new Holiday {  Name = "عيد الأضحى ", Date = new DateTime(DateTime.UtcNow.Year, 6, 17) },

                    // المولد
                    new Holiday {  Name = "المولد النبوي الشريف", Date = new DateTime(DateTime.UtcNow.Year, 9, 15) },

                    // ==================
                    // 🎉 New Yearrnational
                    // ==================

                    new Holiday {  Name = "رأس السنة الميلادية", Date = new DateTime(DateTime.UtcNow.Year, 1, 1) }

                );

                await context.SaveChangesAsync();
            }

            // =========================
            // 🖼 IMAGES
            // =========================
            if (!await context.ChaletImages.AnyAsync())
            {
                context.ChaletImages.AddRange(
                    new ChaletImage { ChaletId = 101, ImageUrl = "101.PNG" },
                    new ChaletImage { ChaletId = 102, ImageUrl = "102.PNG" },
                    new ChaletImage { ChaletId = 103, ImageUrl = "103.PNG" },
                    new ChaletImage { ChaletId = 104, ImageUrl = "104.PNG" },
                    new ChaletImage { ChaletId = 105, ImageUrl = "105.PNG" },
                    new ChaletImage { ChaletId = 106, ImageUrl = "106.PNG" },
                    new ChaletImage { ChaletId = 107, ImageUrl = "107.PNG" },
                    new ChaletImage { ChaletId = 108, ImageUrl = "108.PNG" }
                );

                await context.SaveChangesAsync();
            }

            // =========================
            // 🤝 OWNERS (بنفس نظامك)
            // =========================
            
            if (!await context.ChaletOwners.AnyAsync())
            {
                var chalets = await context.Chalets.ToListAsync();

                var owners = new List<ChaletOwner>();

                foreach (var chalet in chalets.Take(8))
                {
                    owners.Add(new ChaletOwner
                    {
                        ChaletId = chalet.Id,
                        UserId = admin.Id,
                        SharePercentage = 50
                    });
                }

                foreach (var chalet in chalets.Take(4))
                {
                    owners.Add(new ChaletOwner
                    {
                        ChaletId = chalet.Id,
                        UserId = partner1.Id,
                        SharePercentage = 50
                    });
                }

                foreach (var chalet in chalets.Skip(4).Take(4))
                {
                    owners.Add(new ChaletOwner
                    {
                        ChaletId = chalet.Id,
                        UserId = partner2.Id,
                        SharePercentage = 50
                    });
                }

                context.ChaletOwners.AddRange(owners);
                await context.SaveChangesAsync();
            }
        }

        private static async Task<AppUser> EnsureUser(
            UserManager<AppUser> userManager,
            string email,
            string name)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FullName = name,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "Partner@123");
            }

            return user;
        }
    }
}