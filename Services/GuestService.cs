
using HotelManagementEFCore.Models;
using HotelManagementEFCore.Repositories;

namespace HotelManagementEFCore.Services
{
    public class GuestService
    {
        private readonly GuestRepository _repository;

        public GuestService(GuestRepository repository)
        {
            _repository = repository;
        }

        public List<Guest> GetAllGuests()
        {
            return _repository.GetAllGuests();
        }

        public Guest? GetGuestById(int id)
        {
            return _repository.GetGuestById(id);
        }

        public bool RegisterGuest(Guest guest)
        {
            if (string.IsNullOrWhiteSpace(guest.FullName))
                return false;

            if (string.IsNullOrWhiteSpace(guest.PhoneNumber))
                return false;

            if (string.IsNullOrWhiteSpace(guest.Email))
                return false;

            if (!guest.Email.Contains('@') ||
                !guest.Email.Contains('.'))
                return false;

            _repository.AddGuest(guest);
            return true;
        }
    }
}


