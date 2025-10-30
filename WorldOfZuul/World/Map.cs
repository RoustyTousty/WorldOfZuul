namespace WorldOfZuul.World
{
    public class Map
    {
        public List<Location> Locations { get; } = new();

        public Location? GetLocation(string id) => Locations.FirstOrDefault(l => l.Id == id);
    }
}