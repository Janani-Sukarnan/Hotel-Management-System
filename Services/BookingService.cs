
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

            if (booking == null || booking.Status != "Confirmed")
                return false;

            bool cancelled = _repository.CancelBooking(bookingId);

            if (!cancelled)
                return false;

            // Check whether another confirmed booking remains for this room.
            bool hasOtherConfirmedBooking = _context.Bookings.Any(b =>
                b.RoomId == booking.RoomId &&
                b.Status == "Confirmed" &&
                b.Id != bookingId);

            var room = _context.Rooms.Find(booking.RoomId);

            if (room != null)
            {
                room.IsAvailable = !hasOtherConfirmedBooking;
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
            // Validate booking dates.
            if (checkIn.Date < DateTime.Today)
                return false;

            if (checkOut.Date <= checkIn.Date)
                return false;

            // Check that the guest and room exist.
            var guest = _context.Guests.Find(guestId);
            var room = _context.Rooms.Find(roomId);

            if (guest == null || room == null)
                return false;

            // Prevent overlapping confirmed bookings for this room.
            bool hasOverlap = _context.Bookings.Any(b =>
                b.RoomId == roomId &&
                b.Status == "Confirmed" &&
                checkIn.Date < b.CheckOutDate.Date &&
                checkOut.Date > b.CheckInDate.Date);

            if (hasOverlap)
                return false;

            // Calculate the booking amount.
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

            _repository.AddBooking(booking);

            return true;
        }
    }
}
        