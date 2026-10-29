
using HotelManagementEFCore.Models;
using HotelManagementEFCore.Repositories;

namespace HotelManagementEFCore.Services
{
    public class RoomService
    {
        private readonly RoomRepository _repository;

        public RoomService(RoomRepository repository)
        {
            _repository = repository;
        }

        public List<Room> GetAllRooms()
        {
            return _repository.GetAllRooms();
        }

        public bool AddRoom(Room room)
        {
            if (string.IsNullOrWhiteSpace(room.RoomNumber))
                return false;

            if (string.IsNullOrWhiteSpace(room.RoomType))
                return false;

            if (room.PricePerNight <= 0)
                return false;

            if (_repository.GetRoomByNumber(room.RoomNumber) != null)
                return false;

            _repository.AddRoom(room);
            return true;
        }

        public Room? SearchRoom(string roomNumber)
        {
            return _repository.GetRoomByNumber(roomNumber);
        }

        public bool DeleteRoom(int id)
        {
            return _repository.DeleteRoom(id);
        }
    }
}
