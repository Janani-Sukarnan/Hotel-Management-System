
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

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .OrderBy(b => b.Id)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await GetBookingByIdAsync(bookingId);

            if (booking == null)
                return false;

            if (booking.Status == "Cancelled")
                return false;

            booking.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
