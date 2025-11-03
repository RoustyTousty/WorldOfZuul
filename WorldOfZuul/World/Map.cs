namespace WorldOfZuul.World
{
    /*
    * Map class representing the entire game world.
    * Contains all Locations.
    */
    public class Map
    {
        public string StartingLocationId { get; set; }
        public Dictionary<string, Location> Locations { get; } = new();

        public Map(string startingLocationId)
        {
            StartingLocationId = startingLocationId;
        }

        /*
        * Returns a location by its ID.
        */
        public Location? GetLocation(string id)
        {
            if (Locations.TryGetValue(id, out var location))
            {
                return location;
            }
            return null;
        }



        /*
        * Sets a new location to the map.
        */
        public void SetLocation(Location location)
        {
            if (location == null) return;
            Locations[location.Id] = location;
        }
    }
}