
using HotelManagementEFCore.Data;
using HotelManagementEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementEFCore.Repositories
{
    public class BookingRepository
    {
        private readonly HotelDbContext _context;

        public BookingRepository(HotelDbContext context)
        {
            _context = context;
        }

        public List<Booking> GetAllBookings()
        {
            return _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .OrderBy(b => b.Id)
                .ToList();
        }

        public Booking? GetBookingById(int id)
        {
            return _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .FirstOrDefault(b => b.Id == id);
        }

        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }


        public bool CancelBooking(int bookingId)
        {
            var booking = _context.Bookings
                .FirstOrDefault(b => b.Id == bookingId);

            if (booking == null)
                return false;

            if (booking.Status == "Cancelled")
                return false;

            booking.Status = "Cancelled";

            _context.SaveChanges();

            return true;
        }

    }
}
