
using HotelManagementEFCore.Data;
using HotelManagementEFCore.Models;
using HotelManagementEFCore.Repositories;
using HotelManagementEFCore.Services;
using HotelManagementEFCore.Menus;

using var db = new HotelDbContextFactory().CreateDbContext(args);

var roomRepository = new RoomRepository(db);
var roomService = new RoomService(roomRepository);

var guestRepository = new GuestRepository(db);
var guestService = new GuestService(guestRepository);
var bookingRepository = new BookingRepository(db);
var bookingService = new BookingService(db, bookingRepository);

// Seed rooms only when no rooms exist.
if (!roomService.GetAllRooms().Any())
{
    roomService.AddRoom(new Room
    {
        RoomNumber = "101",
        RoomType = "Standard",
        PricePerNight = 1500m
    });

    roomService.AddRoom(new Room
    {
        RoomNumber = "102",
        RoomType = "Standard",
        PricePerNight = 1500m
    });

    roomService.AddRoom(new Room
    {
        RoomNumber = "201",
        RoomType = "Deluxe",
        PricePerNight = 2500m
    });

    roomService.AddRoom(new Room
    {
        RoomNumber = "301",
        RoomType = "Premium",
        PricePerNight = 4000m
    });
    
}
var menu = new HotelMenu(
    roomService,
    guestService,
    bookingService);

menu.Run();
