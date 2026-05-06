using AppDAL.Entities;

public class BookingNote
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; }

    public string Note { get; set; }
    public string UserName { get; set; }
    //public int CreatedByUserId { get; set; }
    //public AppUser CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}