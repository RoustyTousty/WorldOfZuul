namespace WorldOfZuul.Item
{
    public class UsableItem : Item
    {
        public string? OnUseEventId { get; set; }

        public UsableItem(string id, string name, string desc = "", string? onUseEventId = null)
            : base(id, name, desc)
        {
            OnUseEventId = onUseEventId;
        }
    }
}
