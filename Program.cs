using HotelManagementEFCore.Data;
using Microsoft.EntityFrameworkCore;

try
{
    using var db =
        new HotelDbContextFactory().CreateDbContext(args);

    Console.WriteLine("Connecting to Hotel Management Database...");

    if (db.Database.CanConnect())
    {
        Console.WriteLine("SQL Server connection successful!");
        Console.WriteLine("Database: " +
            db.Database.GetDbConnection().Database);

        Console.WriteLine("Rooms table ready: " +
            db.Rooms.Any());

        Console.WriteLine("Guests table ready: " +
            db.Guests.Any());

        Console.WriteLine("Bookings table ready: " +
            db.Bookings.Any());
    }
    else
    {
        Console.WriteLine("Could not connect to SQL Server.");
    }
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}