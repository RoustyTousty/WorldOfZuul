namespace WorldOfZuul.World
{
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

        public bool TryUnlock(Item.Item byItem)
        {
            if (!IsLocked) return true;
            if (byItem == null) return false;
            if (KeyItemId != null && byItem.Id == KeyItemId)
            {
                IsLocked = false;
                return true;
            }
            return false;
        }
    }
}
