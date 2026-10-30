
using HotelManagementEFCore.Models;
using HotelManagementEFCore.Repositories;

namespace HotelManagementEFCore.Services
{
    public class GuestService
    {
        private readonly GuestRepository _repository;
        private readonly AuditLogService _auditLogService;

        public GuestService(
            GuestRepository repository,
            AuditLogService auditLogService)
        {
            _repository = repository;
            _auditLogService = auditLogService;
        }

        public async Task<List<Guest>> GetAllGuestsAsync()
        {
            return await _repository.GetAllGuestsAsync();
        }

        public async Task<Guest?> GetGuestByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetGuestByIdAsync(id);
        }

        public async Task<bool> RegisterGuestAsync(Guest guest)
        {
            if (string.IsNullOrWhiteSpace(guest.FullName))
                return false;

            if (string.IsNullOrWhiteSpace(guest.PhoneNumber))
                return false;

            if (string.IsNullOrWhiteSpace(guest.Email))
                return false;

            if (!guest.Email.Contains('@') || !guest.Email.Contains('.'))
                return false;

            await _repository.AddGuestAsync(guest);

            await _auditLogService.LogEventAsync(
                "Guest Registered",
                $"Guest {guest.FullName} was registered successfully."
            );

            return true;
        }
    }
}
