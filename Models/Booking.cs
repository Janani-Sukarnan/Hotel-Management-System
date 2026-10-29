using System;

namespace HotelManagementEFCore.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int GuestId { get; set; }

        public int RoomId { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Confirmed";

        public Guest? Guest { get; set; }

        public Room? Room { get; set; }
    }
}
