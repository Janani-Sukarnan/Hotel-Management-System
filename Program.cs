
using HotelManagementEFCore.Data;
using HotelManagementEFCore.Models;
using HotelManagementEFCore.Repositories;
using HotelManagementEFCore.Services;
using HotelManagementEFCore.Menus;

using var db = new HotelDbContextFactory().CreateDbContext(args);

// Audit Log
var auditLogRepository = new AuditLogRepository(db);
var auditLogService = new AuditLogService(auditLogRepository);

// Room
var roomRepository = new RoomRepository(db);
var roomService = new RoomService(roomRepository, auditLogService);

// Guest
var guestRepository = new GuestRepository(db);
var guestService = new GuestService(guestRepository, auditLogService);

// Booking
var bookingRepository = new BookingRepository(db);
var bookingService = new BookingService(
    db,
    bookingRepository,
    auditLogService
);

// Seed rooms only when no rooms exist.
var existingRooms = await roomService.GetAllRoomsAsync();

if (!existingRooms.Any())
{
    await roomService.AddRoomAsync(new Room
    {
        RoomNumber = "101",
        RoomType = "Standard",
        PricePerNight = 1500m
    });

    await roomService.AddRoomAsync(new Room
    {
        RoomNumber = "102",
        RoomType = "Standard",
        PricePerNight = 1500m
    });

    await roomService.AddRoomAsync(new Room
    {
        RoomNumber = "201",
        RoomType = "Deluxe",
        PricePerNight = 2500m
    });

    await roomService.AddRoomAsync(new Room
    {
        RoomNumber = "301",
        RoomType = "Premium",
        PricePerNight = 4000m
    });
}

var menu = new HotelMenu(
    roomService,
    guestService,
    bookingService
);

await menu.RunAsync();
