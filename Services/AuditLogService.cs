
using HotelManagementEFCore.Models;
using HotelManagementEFCore.Repositories;

namespace HotelManagementEFCore.Services
{
    public class AuditLogService
    {
        private readonly AuditLogRepository _repository;

        public AuditLogService(AuditLogRepository repository)
        {
            _repository = repository;
        }

        public async Task LogEventAsync(string eventType, string description)
        {
            var log = new AuditLog
            {
                EventType = eventType,
                Description = description,
                Timestamp = DateTime.Now
            };

            await _repository.AddLogAsync(log);
        }

        public async Task<List<AuditLog>> GetAllLogsAsync()
        {
            return await _repository.GetAllLogsAsync();
        }
    }
}


