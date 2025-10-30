namespace WorldOfZuul.Events
{
    public interface IEvent
    {
        string Id { get; }
        bool IsActive { get; }
        void Activate(EventManager em);
    }
}