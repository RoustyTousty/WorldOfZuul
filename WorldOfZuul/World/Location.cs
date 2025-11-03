namespace WorldOfZuul.World
{
    /*
    * Location class representing a group of rooms in the game world.
    */
    public class Location
    {
        // TODO: Small refactor. Change how StartingRoom and Rooms are handled.
        public string Id { get; }
        public string Name { get; set; }
        public List<Room> Rooms { get; } = new();

        public Room StartingRoom => Rooms.First();

        public Location(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}