using WorldOfZuul.Entities;
using WorldOfZuul.World;

namespace WorldOfZuul.Events
{
    public interface IEvent
    {
        string Id { get; }
        void Execute(Game game, Player player, Map map);
    }
}