
using System;
using System.Text;
using AppBL.Helpers;
using AppBL.IService;
using AppBL.Mapper;
using AppBL.Service;
using AppDAL.Context;
using AppDAL.Entities;
using AppDAL.IRepo;
using AppDAL.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AppPL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingExtraRepository, BookingExtraRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IHolidayService, HolidayService>();
            builder.Services.AddScoped<IPricingService, PricingService>();
            builder.Services.AddScoped<IChaletRepository, ChaletRepository>();
            builder.Services.AddScoped<IChaletService, ChaletService>();
            builder.Services.AddScoped<IBookingExtraService, BookingExtraService>();
            builder.Services.AddScoped<IWaitingListBookingRepo, WaitingListBookingRepo>();
            builder.Services.AddScoped<IWaitingListService, WaitingListService>();
            builder.Services.AddScoped<IBookingExtraService, BookingExtraService>();
            builder.Services.AddScoped<IChaletOwnerService, ChaletOwnerService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IExtraService, ExtraService>();
            builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IBookingNoteService, BookingNoteService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<RefreshTokenService>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSignalR();

            builder.Services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<HotelDbContext>()
                .AddDefaultTokenProviders();

            // Add services to the container.
            builder.Services.AddDbContext<HotelDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            //شفل هيتشال 
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

                // 🔐 JWT Auth in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var services = scope.ServiceProvider;

                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                    var context = services.GetRequiredService<HotelDbContext>();

                    await DbSeeder.SeedAsync(userManager, roleManager, context);

                    Console.WriteLine("SEED COMPLETED SUCCESSFULLY");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SEED FAILED: " + ex.Message);
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapHub<NotificationHub>("/hubs/notifications");
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();


            app.MapControllers();

            app.Run();
        }
    }
}
