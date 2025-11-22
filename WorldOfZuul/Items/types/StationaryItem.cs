namespace WorldOfZuul.Items
{
    public class StationaryItem : Item
    {
        public override bool CanPickUp => false;
        
        public StationaryItem(string id, string name, string description, string? useText = null)
            : base(id, name, description, useText)
        {
            
        }
    }
}
