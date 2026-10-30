
using HotelManagementEFCore.Data;
using HotelManagementEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementEFCore.Repositories
{
    public class GuestRepository
    {
        private readonly HotelDbContext _context;

        public GuestRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<Guest>> GetAllGuestsAsync()
        {
            return await _context.Guests
                .AsNoTracking()
                .OrderBy(g => g.Id)
                .ToListAsync();
        }

        public async Task<Guest?> GetGuestByIdAsync(int id)
        {
            return await _context.Guests
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddGuestAsync(Guest guest)
        {
            await _context.Guests.AddAsync(guest);
            await _context.SaveChangesAsync();
        }
    }
}
