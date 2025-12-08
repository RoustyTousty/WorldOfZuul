namespace WorldOfZuul.Items
{
    /*
     * Represents an item that can be used by the player.
     */
    public class UsableItem : Item
        {

            
            public UsableItem(string id, string name, string description, string? useText=null)
                : base(id, name, description, useText)
        {
        }

        public override bool Use()
        {
            if (!string.IsNullOrWhiteSpace(UseText))
                {
                Console.WriteLine(UseText);
                return true;      // Use succeeded - item has text to display
                }
            else
                {
                Console.WriteLine("nothing happens...");
                return false;     // Use failed - no text, nothing happened
                }
        }

        }
}

