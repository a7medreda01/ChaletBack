using AppDAL.Entities;

public class Maintenance
{
    public int Id { get; set; }
    public int ChaletId { get; set; }

    public string Description { get; set; }
    public MaintenanceStatus Status { get; set; }
    public Chalet Chalet { get; set; }
    public DateTime CreatedAt { get; set; }
}