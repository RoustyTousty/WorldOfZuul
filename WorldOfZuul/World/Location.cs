namespace WorldOfZuul.World
{
    /*
    * Location class representing a group of rooms in the game world.
    */
    public class Location
    {
        public string Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartingRoomId { get; set; }

        public Dictionary<string, Room> Rooms { get; } = new();

        public Location(string id, string name, string description, string startingRoomId)
        {
            Id = id;
            Name = name;
            Description = description;
            StartingRoomId = startingRoomId;
        }



        /*
        * Returns a room by its ID.
        */
        public Room? GetRoom(string id)
        {
            if (Rooms.TryGetValue(id, out var room))
            {
                return room;
            }
            return null;
        }



        /*
        * Sets a new room to the location.
        */
        public void SetRoom(Room room)
        {
            if (room == null) return;
            Rooms[room.Id] = room;
        }
    }
}