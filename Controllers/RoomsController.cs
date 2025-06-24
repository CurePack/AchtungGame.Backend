using AchtungGame.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public ActionResult<List<Room>> GetRooms()
    {
        return Ok(_roomService.GetAll());
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RoomId))
            return BadRequest("RoomId is required.");

        if (request.Player == null || string.IsNullOrWhiteSpace(request.Player.Name))
            return BadRequest("Host player information is required.");

        var createdRoom = new Room
        {
            RoomId = request.RoomId,
            RoomMaxPlayers = request.RoomMaxPlayers,
            RoomPassword = request.RoomPassword,
            RoomHostName = request.RoomHostName,
            Players = new List<Player> { request.Player }
        };

        bool success = await _roomService.AddRoomAsync(createdRoom);

        if (!success)
            return Conflict("Room already exists.");

        return Ok(createdRoom);
    }

    [HttpPost("{roomId}/join")]
    public async Task<IActionResult> JoinRoom(string roomId, [FromBody] Player player)
    {
        bool success = await _roomService.AddPlayerToRoomAsync(roomId, player);

        if (success)
            return Ok();

        return Conflict("Player already in room or room not found");
    }

    [HttpPost("{roomId}/leave")]
    public async Task<IActionResult> LeaveRoom(string roomId, [FromBody] Player player)
    {
        bool success = await _roomService.RemovePlayerFromRoomAsync(roomId, player);

        if (success)
            return Ok();

        return NotFound("Player or room not found");
    }
}
