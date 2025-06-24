using AchtungGame.Backend.Hubs;
using AchtungGame.Backend.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchtungGame.Backend.Services
{
    public class RoomService : IRoomService
    {
        private readonly Dictionary<string, Room> _rooms = new();
        private readonly IHubContext<RoomHub> _hubContext;

        public RoomService(IHubContext<RoomHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public List<Room> GetAll() => _rooms.Values.ToList();

        public async Task<Room> CreateRoomAsync(string roomId, int maxPlayers, string password, string hostName)
        {
            var room = new Room
            {
                RoomId = roomId,
                RoomMaxPlayers = maxPlayers,
                RoomPassword = password,
                RoomHostName = hostName,
                Players = new List<Player>()
            };

            _rooms[roomId] = room;
            await _hubContext.Clients.All.SendAsync("RoomCreated", room);
            return room;
        }

        public Room? GetRoom(string roomId) =>
            _rooms.TryGetValue(roomId, out var room) ? room : null;

        public async Task<bool> AddRoomAsync(Room room)
        {
            if (_rooms.ContainsKey(room.RoomId))
                return false;

            _rooms.Add(room.RoomId, room);
            await _hubContext.Clients.All.SendAsync("RoomCreated", room);
            return true;
        }

        public async Task<bool> AddPlayerToRoomAsync(string roomId, Player player)
        {
            if (_rooms.TryGetValue(roomId, out var room))
            {
                if (room.Players.Any(p => p.Name == player.Name))
                    return false;

                room.Players.Add(player);

                await _hubContext.Clients.Group(roomId).SendAsync("PlayerJoined", roomId, player);
                await _hubContext.Clients.All.SendAsync("RoomUpdated", room); // Broadcast updated room list

                return true;
            }

            return false;
        }

        public async Task<bool> RemovePlayerFromRoomAsync(string roomId, Player player)
        {
            if (_rooms.TryGetValue(roomId, out var room))
            {
                var existingPlayer = room.Players.FirstOrDefault(p => p.Name == player.Name);

                if (existingPlayer != null)
                {
                    room.Players.Remove(existingPlayer);

                    await _hubContext.Clients.Group(roomId).SendAsync("PlayerLeft", roomId, player);

                    if (room.Players.Count == 0)
                    {
                        _rooms.Remove(roomId);
                        await _hubContext.Clients.All.SendAsync("RoomDeleted", roomId);
                    }
                    else
                    {
                        await _hubContext.Clients.All.SendAsync("RoomUpdated", room);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
