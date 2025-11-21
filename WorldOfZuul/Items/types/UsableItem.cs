namespace WorldOfZuul.Items
{

    
    public class UsableItem : Item
        {

            
            public UsableItem(string id, string name, string description, string? useText=null)
                : base(id, name, description, useText)
        {
        }

        public override void Use()
        {
            if (!string.IsNullOrWhiteSpace(UseText))
                Console.WriteLine(UseText);
            else
                Console.WriteLine("nothing happens...");
        }
    }

}

