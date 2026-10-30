
namespace HotelManagementEFCore.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string EventType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
