
using Microsoft.EntityFrameworkCore;
using HotelManagementEFCore.Data;
using HotelManagementEFCore.Models;

namespace HotelManagementEFCore.Repositories
{
    public class RoomRepository
    {
        private readonly HotelDbContext _context;

        public RoomRepository(HotelDbContext context)
        {
            _context = context;
        }

        public List<Room> GetAllRooms()
        {
            return _context.Rooms
                .OrderBy(r => r.RoomNumber)
                .ToList();
        }

        public Room? GetRoomById(int id)
        {
            return _context.Rooms
                .FirstOrDefault(r => r.Id == id);
        }

        public Room? GetRoomByNumber(string roomNumber)
        {
            return _context.Rooms
                .FirstOrDefault(r => r.RoomNumber == roomNumber);
        }

        public void AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public bool DeleteRoom(int id)
        {
            var room = GetRoomById(id);

            if (room == null)
                return false;

            bool hasBookings = _context.Bookings
                .Any(b => b.RoomId == id);

            if (hasBookings)
                return false;

            _context.Rooms.Remove(room);
            _context.SaveChanges();

            return true;
        }
    }
}
