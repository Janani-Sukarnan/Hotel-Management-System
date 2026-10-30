
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

        public async Task<List<Room>> GetAllRoomsAsync()
        {
             return await _context.Rooms
                .OrderBy(r => r.RoomNumber)
                .ToListAsync    ();
        } 

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);
        }
         
        

        public async Task<Room?> GetRoomByNumberAsync(string roomNumber)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
        }

        public async Task AddRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }
        //for delete use Task<bool> DeleteRoomAsync(int id)
        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await GetRoomByIdAsync(id);

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
