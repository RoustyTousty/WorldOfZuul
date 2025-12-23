using WorldOfZuul.Entities;
using WorldOfZuul.World;

namespace WorldOfZuul.Events
{
    public class EventManager
    {
        public static EventManager Instance { get; } = new EventManager();

        private readonly Dictionary<string, IEvent> Events = new Dictionary<string, IEvent>();

        private Game? Game;
        private Player? Player;
        private Map? Map;

        private EventManager() { }

        public void Initialize(Game game, Player player, Map map)
        {
            Game = game;
            Player = player;
            Map = map;
        }

        public void Register(IEvent ev)
        {
            Events[ev.Id] = ev;
        }

        public void Trigger(string eventId)
        {
            if (Events.TryGetValue(eventId, out IEvent? eventInstance))
            {
                eventInstance.Execute(Game!, Player!, Map!);
            }
            else
            {
                Console.WriteLine($"[DEBUG] No event registered with id '{eventId}'.");
            }
        }
    }
}
