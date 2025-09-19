namespace WorldOfZuul
{
    public class RoomKey : Item
    {
        public string UnlocksRoom { get; }

        public RoomKey(string id, string name, string description, string unlocksRoom) : base(id, name, description)
        {
            UnlocksRoom = unlocksRoom;
        }

        public override void Interact(Game game)
        {
            
        }
    }
}