using WorldOfZuul.Items;

namespace WorldOfZuul.World
{
    /*
    * Room class representing a location in the game world.
    */
    public class Room
    {
        public string Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Dictionary<string, Exit> Exits { get; } = new();
        public List<Item> Items { get; } = new();
        // public List<Entities.Npc> Npcs { get; } = new();
        // public List<Events.IEvent> Triggers { get; } = new();

        public Room(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }



        /*
        * Get the exit that matches the given name.
        * Returns null if the exit does not exist.
        */
        public Exit? GetExit(string name)
        {
            if (Exits.TryGetValue(name, out var exit))
            {
                return exit;
            }
            return null;
        }



        /*
        * Sets a new exit for the room.
        */
        public void SetExit(string name, Exit exit)
        {
            if (name == null || exit == null) return;
            Exits[name] = exit;
        }
    }
}
