namespace AchtungGame.Backend.Models
{
    public class Room
    {
        public string RoomId { get; set; }
        public int RoomMaxPlayers { get; set; }
        public string RoomPassword { get; set; }
        public string RoomHostName { get; set; }
        public List<Player> Players { get; set; } = new();
    }
}
