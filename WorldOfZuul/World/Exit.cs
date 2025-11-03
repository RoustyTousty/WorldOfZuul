namespace WorldOfZuul.World
{
    /*
    * Exit class representing a connection from one room to another.
    * Acts as a doorway or path between rooms, but works one way and can be locked.
    */
    public class Exit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Room TargetRoom { get; set; }
        public bool IsLocked { get; private set; } = false;
        public string? KeyItemId { get; set; }

        public Exit(string id, string name, Room targetRoom, bool isLocked = false, string? keyItemId = null)
        {
            Id = id;
            Name = name;
            TargetRoom = targetRoom;
            IsLocked = isLocked;
            KeyItemId = keyItemId;
        }




        

        /*
        * Unlocks the exit.
        */
        public void Unlock()
        {
            IsLocked = false;
        }
    }
}
