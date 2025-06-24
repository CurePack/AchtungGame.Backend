using AchtungGame.Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRoomService
{
    List<Room> GetAll();
    Task<Room> CreateRoomAsync(string roomId, int maxPlayers, string password, string hostName);
    Room? GetRoom(string roomId);
    Task<bool> AddRoomAsync(Room room);
    Task<bool> AddPlayerToRoomAsync(string roomId, Player player);
    Task<bool> RemovePlayerFromRoomAsync(string roomId, Player player);
}
