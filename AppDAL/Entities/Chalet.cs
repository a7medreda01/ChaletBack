using AppDAL.Entities;

public class Chalet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ChaletType Type { get; set; }
    public ChaletStatus Status { get; set; }

    public bool HasMorning { get; set; }
    public bool HasEvening { get; set; }
    public bool HasFullDay { get; set; }

    public ICollection<ChaletOwner> ChaletOwners { get; set; }
    public ICollection<ChaletImage> Images { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}