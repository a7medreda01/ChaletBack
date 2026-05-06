using AppDAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppDAL.Context
{
    public class HotelDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        // 🧱 DbSets
        public DbSet<Chalet> Chalets { get; set; }
        public DbSet<ChaletImage> ChaletImages { get; set; }
        public DbSet<ChaletOwner> ChaletOwners { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingExtra> BookingExtras { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Extra> Extras { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Pricing> Pricings { get; set; }

        //public DbSet<Partner> Partners { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<WaitingList> WaitingLists { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChaletOwner>()
                .HasOne(co => co.Chalet)
                .WithMany(c => c.ChaletOwners)
                .HasForeignKey(co => co.ChaletId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChaletOwner>()
                .HasOne(co => co.User)
                .WithMany(u => u.ChaletOwners)
                .HasForeignKey(co => co.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChaletImage>()
                .HasOne(x => x.Chalet)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ChaletId);

            modelBuilder.Entity<BookingExtra>()
                .HasOne(x => x.Booking)
                .WithMany(x => x.BookingExtras)
                .HasForeignKey(x => x.BookingId);

            modelBuilder.Entity<BookingExtra>()
                .HasOne(x => x.Extra)
                .WithMany()
                .HasForeignKey(x => x.ExtraId);

            modelBuilder.Entity<Payment>()
                .HasOne(x => x.Booking)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.BookingId);

            modelBuilder.Entity<Maintenance>()
                .HasOne(x => x.Chalet)
                .WithMany()
                .HasForeignKey(x => x.ChaletId);

            modelBuilder.Entity<Pricing>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);
            modelBuilder.Entity<Chalet>().Property(x => x.Id)
                .ValueGeneratedNever();

        }
    }
}