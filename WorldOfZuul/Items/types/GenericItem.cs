namespace WorldOfZuul.Items
{
public class GenericItem : Item
    {
        
        public GenericItem(string id, string name, string description)
            : base(id, name, description)
        {
            
        }

        public override void Use()
        {
            Console.WriteLine($"Can't use {Name}.");
        }


    }
}