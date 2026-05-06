using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.Entities
{
    public enum ChaletStatus { Available, Booked, Maintenance }

    public enum BookingStatus { Pending, Confirmed, Cancelled ,Done}

    public enum PaymentStatus { Pending, Paid, Failed }
    public enum PaymentMethod { Cash, Card }

    public enum WaitingStatus
    {
        Pending,
        Contacted,
        Booked,
        Cancelled
    }
    public enum MaintenanceStatus { Open, InProgress, Closed }
    public enum UserRole
    {
        Manager,
        Employee,
        Partner
    }
    public enum BookingPeriod { Morning, Evening, Full }
    public enum ChaletType { Normal, Royal }

    public enum DayType
    {
        Weekday,
        Weekend,
        Holiday
    }
}
