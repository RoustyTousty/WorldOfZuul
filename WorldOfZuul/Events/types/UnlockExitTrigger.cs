namespace WorldOfZuul.Events
{
    public class UnlockExitTrigger : IEvent
    {
        public string Id { get; init; }
        public bool IsActive { get; private set; } = false;
        public string RoomId { get; init; }
        public string ExitName { get; init; }

        public UnlockExitTrigger(string id, string roomId, string exitName)
        {
            Id = id;
            RoomId = roomId;
            ExitName = exitName;
        }

        public void Activate(EventManager em)
        {
            IsActive = true;
        }
    }
}
