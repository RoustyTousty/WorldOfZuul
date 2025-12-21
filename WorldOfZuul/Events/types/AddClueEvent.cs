using WorldOfZuul.Entities;

namespace WorldOfZuul.Events.types
{
    public class AddClueEvent : IEvent
    {
        public string Id { get; }
        private string clueId;

        public AddClueEvent(string id, string clueId)
        {
            Id = id;
            this.clueId = clueId;
        }

        public void Execute(Game game, Player player, World.Map map)
        {
            game.Case.AddClue(clueId);
            Console.WriteLine("This evidence could change everything...");
        }
    }
}
