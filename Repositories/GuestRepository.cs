
using HotelManagementEFCore.Data;
using HotelManagementEFCore.Models;

namespace HotelManagementEFCore.Repositories
{
    public class GuestRepository
    {
        private readonly HotelDbContext _context;

        public GuestRepository(HotelDbContext context)
        {
            _context = context;
        }

        public List<Guest> GetAllGuests()
        {
            return _context.Guests
                .OrderBy(g => g.Id)
                .ToList();
        }

        public Guest? GetGuestById(int id)
        {
            return _context.Guests
                .FirstOrDefault(g => g.Id == id);
        }

        public void AddGuest(Guest guest)
        {
            _context.Guests.Add(guest);
            _context.SaveChanges();
        }
    }
}

