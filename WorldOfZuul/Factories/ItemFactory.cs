using WorldOfZuul.Items;

namespace WorldOfZuul.Factories
{
    public static class ItemFactory
    {
        public static Item Create(ItemData data)
        {
            string typeName = data.Type ?? "GenericItem";
            Type? itemType = Type.GetType($"WorldOfZuul.Items.{typeName}");

            if (itemType == null)
                throw new Exception($"Unknown item type: {typeName}");

            // Resolve constructor
            var ctor = itemType.GetConstructors().First();
            var ctorParams = ctor.GetParameters();

            // Just match typical item constructors,
            // fallback to a generic constructor if needed
            object[] args = ctorParams.Select(p => p.Name switch
            {
                "id" => data.Id,
                "name" => data.Name,
                "description" => data.Description,
                "useText" => data.UseText,
                _ => null!
            }).ToArray();

            Item item = (Item)Activator.CreateInstance(itemType, args)!;

            if (data.Event != null)
                item.EventId = data.Event.Id;

            return item;
        }
    }
}