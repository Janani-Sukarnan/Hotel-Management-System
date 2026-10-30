
using HotelManagementEFCore.Data;
using HotelManagementEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementEFCore.Repositories
{
    public class AuditLogRepository
    {
        private readonly HotelDbContext _context;

        public AuditLogRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(AuditLog log)
        {
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuditLog>> GetAllLogsAsync()
        {
            return await _context.AuditLogs
                .AsNoTracking()
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();
        }
    }
}
