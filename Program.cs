
using HotelManagementEFCore.Data;
using HotelManagementEFCore.Models;
using HotelManagementEFCore.Repositories;
using HotelManagementEFCore.Services;

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

bool running = true;

while (running)
{
    Console.WriteLine("\n========== HOTEL MANAGEMENT SYSTEM ==========");
    Console.WriteLine("1. Display All Rooms");
    Console.WriteLine("2. Search Room by Number");
    Console.WriteLine("3. Add New Room");
    Console.WriteLine("4. Delete Room");
    Console.WriteLine("5. Register Guest");
    Console.WriteLine("6. Display All Guests");
    Console.WriteLine("7. Search Guest by ID");
    Console.WriteLine("8. Create Booking");
    Console.WriteLine("9. Display All Bookings");
    Console.WriteLine("10. Cancel Booking");
    Console.WriteLine("11. Exit");
    Console.Write("Enter your choice: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            var rooms = roomService.GetAllRooms();

            Console.WriteLine("\nID\tRoom No\tType\t\tPrice/Night\tAvailable");

            foreach (var room in rooms)
            {
                Console.WriteLine(
                    $"{room.Id}\t{room.RoomNumber}\t" +
                    $"{room.RoomType}\t\t{room.PricePerNight:C}\t" +
                    $"{(room.IsAvailable ? "Yes" : "No")}");
            }

            if (rooms.Count == 0)
                Console.WriteLine("No rooms found.");

            break;

        case "2":
            Console.Write("Enter room number: ");
            string roomNumber = Console.ReadLine() ?? "";

            var foundRoom = roomService.SearchRoom(roomNumber);

            if (foundRoom != null)
            {
                Console.WriteLine($"Room ID: {foundRoom.Id}");
                Console.WriteLine($"Room Number: {foundRoom.RoomNumber}");
                Console.WriteLine($"Room Type: {foundRoom.RoomType}");
                Console.WriteLine($"Price Per Night: {foundRoom.PricePerNight:C}");
                Console.WriteLine($"Available: {foundRoom.IsAvailable}");
            }
            else
            {
                Console.WriteLine("Room not found.");
            }

            break;

        case "3":
            Console.Write("Enter room number: ");
            string newNumber = Console.ReadLine() ?? "";

            Console.Write("Enter room type: ");
            string newType = Console.ReadLine() ?? "";

            Console.Write("Enter price per night: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Invalid price.");
                break;
            }

            bool added = roomService.AddRoom(new Room
            {
                RoomNumber = newNumber.Trim(),
                RoomType = newType.Trim(),
                PricePerNight = price
            });

            Console.WriteLine(added
                ? "Room added successfully!"
                : "Room not added. Check the details or duplicate room number.");

            break;

        case "4":
            Console.Write("Enter room ID to delete: ");

            if (!int.TryParse(Console.ReadLine(), out int roomId))
            {
                Console.WriteLine("Invalid room ID.");
                break;
            }

            bool deleted = roomService.DeleteRoom(roomId);

            Console.WriteLine(deleted
                ? "Room deleted successfully!"
                : "Room not deleted. It may not exist or may have bookings.");

            break;

        case "5":
            Console.Write("Enter guest name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Enter phone number: ");
            string phone = Console.ReadLine() ?? "";

            Console.Write("Enter email: ");
            string email = Console.ReadLine() ?? "";

            bool registered = guestService.RegisterGuest(new Guest
            {
                FullName = name.Trim(),
                PhoneNumber = phone.Trim(),
                Email = email.Trim()
            });

            Console.WriteLine(registered
                ? "Guest registered successfully!"
                : "Guest registration failed. Check the details.");

            break;

        case "6":
            var guests = guestService.GetAllGuests();

            Console.WriteLine("\nID\tGuest Name\t\tPhone\t\tEmail");

            foreach (var guest in guests)
            {
                Console.WriteLine(
                    $"{guest.Id}\t{guest.FullName}\t\t" +
                    $"{guest.PhoneNumber}\t{guest.Email}");
            }

            if (guests.Count == 0)
                Console.WriteLine("No guests registered.");

            break;

        case "7":
            Console.Write("Enter guest ID: ");

            if (!int.TryParse(Console.ReadLine(), out int guestId))
            {
                Console.WriteLine("Invalid guest ID.");
                break;
            }

            var foundGuest = guestService.GetGuestById(guestId);

            if (foundGuest != null)
            {
                Console.WriteLine($"Guest ID: {foundGuest.Id}");
                Console.WriteLine($"Name: {foundGuest.FullName}");
                Console.WriteLine($"Phone: {foundGuest.PhoneNumber}");
                Console.WriteLine($"Email: {foundGuest.Email}");
            }
            else
            {
                Console.WriteLine("Guest not found.");
            }

            break;


        case "8":
            Console.Write("Enter guest ID: ");

            if (!int.TryParse(Console.ReadLine(), out int bookingGuestId))
            {
                Console.WriteLine("Invalid guest ID.");
                break;
            }

            Console.Write("Enter room ID: ");

            if (!int.TryParse(Console.ReadLine(), out int bookingRoomId))
            {
                Console.WriteLine("Invalid room ID.");
                break;
            }

            Console.Write("Enter check-in date (yyyy-MM-dd): ");

            if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime checkIn))
            {
                Console.WriteLine("Invalid check-in date.");
                break;
            }

            Console.Write("Enter check-out date (yyyy-MM-dd): ");

            if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime checkOut))
            {
                Console.WriteLine("Invalid check-out date.");
                break;
            }

            bool booked = bookingService.CreateBooking(
                bookingGuestId,
                bookingRoomId,
                checkIn,
                checkOut);

            Console.WriteLine(booked
                ? "Booking created successfully!"
                : "Booking failed. Check guest, room, dates, and availability.");

            break;

        case "9":
            var bookings = bookingService.GetAllBookings();

            Console.WriteLine("\n========== BOOKING RECORDS ==========");

            foreach (var booking in bookings)
            {
                Console.WriteLine($"Booking ID: {booking.Id}");
                Console.WriteLine($"Guest: {booking.Guest?.FullName}");
                Console.WriteLine($"Room: {booking.Room?.RoomNumber}");
                Console.WriteLine($"Check-in: {booking.CheckInDate:yyyy-MM-dd}");
                Console.WriteLine($"Check-out: {booking.CheckOutDate:yyyy-MM-dd}");
                Console.WriteLine($"Total Amount: {booking.TotalAmount:C}");
                Console.WriteLine($"Status: {booking.Status}");
                Console.WriteLine("-------------------------------------");
            }

            if (bookings.Count == 0)
                Console.WriteLine("No bookings found.");

            break;

        case "10":
            Console.Write("Enter booking ID to cancel: ");

            if (!int.TryParse(Console.ReadLine(), out int cancelId))
            {
                Console.WriteLine("Invalid booking ID.");
                break;
            }

            bool cancelled = bookingService.CancelBooking(cancelId);

            Console.WriteLine(cancelled
                ? "Booking cancelled successfully! Room is now available."
                : "Cancellation failed. Check the booking ID and status.");

            break;
        case "11":
            running = false;
            Console.WriteLine("Thank you for using Hotel Management System!");
            break;

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}
