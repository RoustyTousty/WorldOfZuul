namespace WorldOfZuul.World
{
    public class Room
    {
        public string Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Dictionary<string, Exit> Exits { get; } = new();

        // public List<Item.Item> Items { get; } = new();
        // public List<Entities.Npc> Npcs { get; } = new();
        // public List<Events.IEvent> Triggers { get; } = new();

        public Room(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Room GetExitsRoom(string name)
        {
            return Exits[name].TargetRoom;
        }

        public void SetExit(string name, Exit exit)
        {
            if (string.IsNullOrWhiteSpace(name) || exit == null) return;
            Exits[name] = exit;
        }

        public bool TryGetExit(string name, out Exit exit)
        {
            if (name == null) { exit = null; return false; }
            return Exits.TryGetValue(name, out exit);
        }
    }
}
