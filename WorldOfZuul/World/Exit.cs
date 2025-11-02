namespace WorldOfZuul.World
{
    /*
    * Exit class representing a connection from one room to another.
    * Acts as a doorway or path between rooms, but works one way and can be locked.
    */
    public class Exit
    {
        // TODO: Small refactor and add Ids to exits.
        public string Name { get; set; }
        public Room TargetRoom { get; set; }
        public bool IsLocked { get; private set; } = false;
        public string? KeyItemId { get; set; }

        public Exit(string name, Room targetRoom, bool isLocked = false, string? keyItemId = null)
        {
            Name = name;
            TargetRoom = targetRoom;
            IsLocked = isLocked;
            KeyItemId = keyItemId;
        }



        /*
        * Unlocks the exit. (method for encapsulating unlocking)
        */
        public void Unlock()
        {
            IsLocked = false;
        }
    }
}
