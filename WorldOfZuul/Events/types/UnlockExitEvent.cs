using WorldOfZuul.World;
using WorldOfZuul.Entities;

namespace WorldOfZuul.Events.types
{
    public class UnlockExitEvent : IEvent
    {
        public string Id { get; }
        private string _roomId;
        private string _exitId;

        public UnlockExitEvent(string id, string roomId, string exitId)
        {
            Id = id;
            _roomId = roomId;
            _exitId = exitId;
        }

        public void Execute(Game game, Player player, Map map)
        {
            Room? room = player.CurrentLocation.GetRoom(_roomId);

            if (room == null)
            {
                Console.WriteLine($"[Event] Room '{_roomId}' not found.");
                return;
            }

            Exit? exit = room.GetExit(_exitId);

            if (exit == null)
            {
                Console.WriteLine($"[Event] Exit '{_exitId}' not found.");
                return;
            }

            exit.Unlock();

            Console.WriteLine($"You hear a loud click... The exit '{exit.Name}' is now unlocked!");
        }
    }
}
