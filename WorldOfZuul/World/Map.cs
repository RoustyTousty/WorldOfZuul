namespace WorldOfZuul.World
{
    /*
    * Map class representing the entire game world.
    * Contains all Locations.
    */
    public class Map
    {
        public List<Location> Locations { get; } = new();

        /*
        * Returns a location by its ID.
        */
        public Location? GetLocation(string id)
        {
            return Locations.FirstOrDefault(l => l.Id == id);
        }
    }
}