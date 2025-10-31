using WorldOfZuul.Item;

namespace WorldOfZuul.World
{
    /*
    * Exit class representing a connection from one room to another.
    * Acts as a doorway or path between rooms.
    * Works one way.
    */
    public class Exit
    {
        public string Name { get; set; }
        public Room TargetRoom { get; set; }
        public bool IsLocked { get; private set; } = false;
        public string? KeyItemId { get; set; }

        public Exit(string name, Room targetRoom, bool locked = false, string? keyItemId = null)
        {
            Name = name;
            TargetRoom = targetRoom;
            IsLocked = locked;
            KeyItemId = keyItemId;
        }

        /*
        * Attempts to unlock the exit using the provided item.
        * TODO: Implement item checking logic.
        */
        // public bool TryUnlock(Item byItem)
        // {
        //     if (!IsLocked) return true;
        //     if (byItem == null) return false;
        //     if (KeyItemId != null && byItem.Id == KeyItemId)
        //     {
        //         IsLocked = false;
        //         return true;
        //     }
        //     return false;
        // }
    }
}
