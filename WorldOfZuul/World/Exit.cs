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
        public Room? TargetRoom { get; set; }
        public Location? TargetLocation { get; }
        public bool IsLocked { get; private set; } = false;

        public Exit(string id, string name, Room? targetRoom = null, Location? targetLocation = null, bool isLocked = false)
        {
            Id = id;
            Name = name;
            TargetRoom = targetRoom;
            TargetLocation = targetLocation;
            IsLocked = isLocked;
        }



        /*
        * Locks the exit.
        */
        public void Lock()
        {
            IsLocked = true;
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
