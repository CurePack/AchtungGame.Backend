using AchtungGame.Backend.Models;

namespace AchtungGame.Backend.GraphQL
{
    public class Query
    {
        public IEnumerable<Room> GetRooms([Service] IRoomService roomService)
            => roomService.GetAll();
    }
}