namespace AchtungGame.Backend.Models
{
    public class CreateRoomRequest
    {
        public string RoomId { get; set; } = string.Empty;
        public int RoomMaxPlayers { get; set; }
        public string RoomPassword { get; set; } = string.Empty;
        public string RoomHostName { get; set; } = string.Empty;
        public Player Player { get; set; } = new Player();
    }
}
