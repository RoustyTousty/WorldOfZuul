using System.Text.Json;
using WorldOfZuul.World;

namespace WorldOfZuul
{
    public class Builder
    {
        private static readonly string DEFAULT_JSON_PATH = Path.Combine(AppContext.BaseDirectory, "Data", "Game.json");

        public Map BuildMapFromJSON(string? filePath = null)
        {
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

            foreach (var locData in worldData.Locations)
            {
                Location location = new Location(locData.Id) { Name = locData.Name };
                Dictionary<string, Room> roomLookup = new Dictionary<string, Room>();

                // Create rooms
                foreach (var roomData in locData.Rooms)
                {
                    Room room = new Room(roomData.Id, roomData.Name, roomData.Description);
                    roomLookup[roomData.Id] = room;
                    location.Rooms.Add(room);
                }

                // Create exits
                foreach (var roomData in locData.Rooms)
                {
                    Room room = roomLookup[roomData.Id];
                    if (roomData.Exits != null)
                    {
                        foreach (var exitData in roomData.Exits)
                        {
                            if (roomLookup.TryGetValue(exitData.TargetRoomId, out Room targetRoom))
                            {
                                room.SetExit(exitData.Name, new Exit(exitData.Name, targetRoom));
                            }
                        }
                    }
                }

                world.Locations.Add(location);
            }

            return world;
        }
    }

    // JSON helper classes
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
    }

    public class ExitData
    {
        public string Name { get; set; } = string.Empty;
        public string TargetRoomId { get; set; } = string.Empty;
    }
}
