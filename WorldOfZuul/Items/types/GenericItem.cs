namespace WorldOfZuul.Items
{
public class GenericItem : Item
    {
        
        public GenericItem(string id, string name, string description, string? useText = null)
            : base(id, name, description, useText)
        {
            
        }

        public override void Use()
        {
            Console.WriteLine($"Can't use {Name}.");
        }


    }
}