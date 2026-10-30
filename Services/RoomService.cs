
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

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            return await _repository.GetAllRoomsAsync();
        }

        public async Task<bool> AddRoomAsync(Room room)
        {
            if (string.IsNullOrWhiteSpace(room.RoomNumber))
                return false;

            if (string.IsNullOrWhiteSpace(room.RoomType))
                return false;

            if (room.PricePerNight <= 0)
                return false;

            var existingRoom =
                await _repository.GetRoomByNumberAsync(room.RoomNumber);

            if (existingRoom != null)
                return false;

            await _repository.AddRoomAsync(room);

            return true;
        }

        public async Task<Room?> SearchRoomAsync(string roomNumber)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
                return null;

            return await _repository.GetRoomByNumberAsync(roomNumber);
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _repository.DeleteRoomAsync(id);
        }
    }
}
