
using HotelManagementEFCore.Data;
using HotelManagementEFCore.Models;
using HotelManagementEFCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementEFCore.Services
{
    public class BookingService
    {
        private readonly HotelDbContext _context;
        private readonly BookingRepository _repository;

        public BookingService(
            HotelDbContext context,
            BookingRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _repository.GetAllBookingsAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetBookingByIdAsync(id);
        }

        public async Task<Booking?> SearchBookingAsync(int bookingId)
        {
            return await GetBookingByIdAsync(bookingId);
        }

        public async Task<bool> CreateBookingAsync(
            int guestId,
            int roomId,
            DateTime checkIn,
            DateTime checkOut)
        {
            if (checkIn.Date < DateTime.Today)
                return false;

            if (checkOut.Date <= checkIn.Date)
                return false;

            var guest = await _context.Guests.FindAsync(guestId);
            var room = await _context.Rooms.FindAsync(roomId);

            if (guest == null || room == null)
                return false;

            bool hasOverlap = await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                b.Status == "Confirmed" &&
                checkIn.Date < b.CheckOutDate.Date &&
                checkOut.Date > b.CheckInDate.Date);

            if (hasOverlap)
                return false;

            int nights = (checkOut.Date - checkIn.Date).Days;
            decimal total = nights * room.PricePerNight;

            var booking = new Booking
            {
                GuestId = guestId,
                RoomId = roomId,
                CheckInDate = checkIn.Date,
                CheckOutDate = checkOut.Date,
                TotalAmount = total,
                Status = "Confirmed"
            };

            await _repository.AddBookingAsync(booking);

            return true;
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _repository.GetBookingByIdAsync(bookingId);

            if (booking == null || booking.Status != "Confirmed")
                return false;

            bool cancelled = await _repository.CancelBookingAsync(bookingId);

            if (!cancelled)
                return false;

            bool hasOtherConfirmedBooking = await _context.Bookings.AnyAsync(b =>
                b.RoomId == booking.RoomId &&
                b.Status == "Confirmed" &&
                b.Id != bookingId);

            var room = await _context.Rooms.FindAsync(booking.RoomId);

            if (room != null)
            {
                room.IsAvailable = !hasOtherConfirmedBooking;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> IsRoomAvailableAsync(
            int roomId,
            DateTime checkIn,
            DateTime checkOut)
        {
            if (checkOut.Date <= checkIn.Date ||
                checkIn.Date < DateTime.Today)
                return false;

            bool roomExists = await _context.Rooms
                .AnyAsync(r => r.Id == roomId);

            if (!roomExists)
                return false;

            bool hasOverlap = await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                b.Status == "Confirmed" &&
                checkIn.Date < b.CheckOutDate.Date &&
                checkOut.Date > b.CheckInDate.Date);

            return !hasOverlap;
        }
    }
}
