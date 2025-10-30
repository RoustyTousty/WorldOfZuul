namespace WorldOfZuul.Events
{
    public class EventManager
    {
        private readonly Dictionary<string, List<Action>> subscribers = new();

        public void Subscribe(string eventId, Action handler)
        {
            if (!subscribers.ContainsKey(eventId)) subscribers[eventId] = new List<Action>();
            subscribers[eventId].Add(handler);
        }

        public void Unsubscribe(string eventId, Action handler)
        {
            if (!subscribers.ContainsKey(eventId)) return;
            subscribers[eventId].Remove(handler);
        }

        public void Publish(string eventId)
        {
            if (!subscribers.ContainsKey(eventId)) return;
            foreach (var handler in subscribers[eventId].ToArray())
                handler?.Invoke();
        }
    }
}