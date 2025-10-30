namespace WorldOfZuul.World
{
    public class Location
    {
        public string Id { get; }
        public string Name { get; set; }
        public List<Room> Rooms { get; } = new();

        public Room StartingRoom => Rooms.First();

        public Location(string id)
        {
            Id = id;
        }
    }
}