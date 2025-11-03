namespace WorldOfZuul.World
{
    /*
    * Map class representing the entire game world.
    * Contains all Locations.
    */
    public class Map
    {
        // TODO: Consider refactoring to use a dictionary for faster lookups. And add a constructur.
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