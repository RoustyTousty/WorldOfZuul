using System.Text.Json;
using WorldOfZuul.World;
using WorldOfZuul.Items;

namespace WorldOfZuul
{
    /*
    * Builder class responsible for constructing the game world from a JSON file.
    */
    public class Builder
    {
        private static string DEFAULT_JSON_PATH = Path.Combine(AppContext.BaseDirectory, "Data", "World.json");

        public Map BuildMapFromJSON(string? filePath = null)
        {
            /*
            * Setup file path and read JSON content.
            */
            if (filePath == null)
                filePath = DEFAULT_JSON_PATH;

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"JSON file not found at {Path.GetFullPath(filePath)}");

            string json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var worldData = JsonSerializer.Deserialize<WorldData>(json, options);
            if (worldData == null)
                throw new Exception("Failed to deserialize world JSON.");

            Map world = new Map();

            /*
            * Reads locations from JSON and creates them in the world.
            */
            foreach (var locData in worldData.Locations)
            {
                Location location = new Location(locData.Id, locData.Name);
                Dictionary<string, Room> roomLookup = new Dictionary<string, Room>();

                /*
                * Reads rooms from JSON and creates them under the location.
                */
                foreach (var roomData in locData.Rooms)
                {
                    Room room = new Room(
                        roomData.Id,
                        roomData.Name,
                        roomData.Description
                    );
                    roomLookup[roomData.Id] = room;
                    location.Rooms.Add(room);
                }

                /*
                * Reads exits from JSON and links rooms together.
                */
                foreach (var roomData in locData.Rooms)
                {
                    Room room = roomLookup[roomData.Id];
                    if (roomData.Exits != null)
                    {
                        foreach (var exitData in roomData.Exits)
                        {
                            if (roomLookup.TryGetValue(exitData.TargetRoomId, out var targetRoom))
                            {
                                Exit exit = new Exit(
                                    exitData.Name,
                                    targetRoom,
                                    exitData.IsLocked ?? false
                                );
                                room.SetExit(exitData.Name, exit);
                            }
                        }
                    }
                }

                /*
                * Reads items from JSON and places them in the appropriate rooms.
                * TODO: Implement item type based builder.
                */
                foreach (var roomData in locData.Rooms)
                {
                    Room room = roomLookup[roomData.Id];
                    if (roomData.Items != null)
                    {
                        foreach (var itemData in roomData.Items)
                        {
                            Item item = new Item(
                                itemData.Id,
                                itemData.Name,
                                itemData.Description
                            );
                            room.Items.Add(item);
                        }
                    }
                }

                // TODO: Create NPCs (Once we have npcs)

                world.Locations.Add(location);
            }

            return world;
        }
    }



    /*    
    * Data classes for constructing the world structure from JSON.
    */
    public class WorldData
    {
        public List<LocationData> Locations { get; set; } = new List<LocationData>();
    }
    public class LocationData
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<RoomData> Rooms { get; set; } = new List<RoomData>();
    }
    public class RoomData
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ExitData>? Exits { get; set; }
        public List<ItemData>? Items { get; set; }
    }
    public class ItemData
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    public class ExitData
    {
        public string Name { get; set; } = string.Empty;
        public string TargetRoomId { get; set; } = string.Empty;
        public bool? IsLocked { get; set; }
    }
}