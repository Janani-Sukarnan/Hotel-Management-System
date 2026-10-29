
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
        public Booking? SearchBooking(int bookingId)
        {
            if (bookingId <= 0)
                return null;

            return _repository.GetBookingById(bookingId);
        }
        public Booking? GetBookingById(int id) =>
    _context.Bookings
        .Include(b => b.Guest)
        .Include(b => b.Room)
        .FirstOrDefault(b => b.Id == id);
        public bool CancelBooking(int bookingId)
        {
            var booking = _repository.GetBookingById(bookingId);

            if (booking == null)
                return false;

            if (booking.Status != "Confirmed")
                return false;

            var result = _repository.CancelBooking(bookingId);

            if (!result)
                return false;

            var room = _context.Rooms.Find(booking.RoomId);

            if (room != null)
            {
                room.IsAvailable = true;
                _context.SaveChanges();
            }

            return true;
        }

        

        public List<Booking> GetAllBookings()
        {
            return _repository.GetAllBookings();
        }

        public bool CreateBooking(
            int guestId,
            int roomId,
            DateTime checkIn,
            DateTime checkOut)
        {
            if (checkIn.Date < DateTime.Today)
                return false;

            if (checkOut.Date <= checkIn.Date)
                return false;

            var guest = _context.Guests.Find(guestId);
            var room = _context.Rooms.Find(roomId);

            if (guest == null || room == null)
                return false;

            if (!room.IsAvailable)
                return false;

            int nights = (checkOut.Date - checkIn.Date).Days;

            decimal total = nights * room.PricePerNight;

            var booking = new Booking
            {
                GuestId = guestId,
                RoomId = roomId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                TotalAmount = total,
                Status = "Confirmed"
            };

            room.IsAvailable = false;

            _repository.AddBooking(booking);
            _context.SaveChanges();

            return true;
        }
    }
}
