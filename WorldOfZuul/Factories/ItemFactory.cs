using WorldOfZuul.Items;

namespace WorldOfZuul.Factories
{
    public static class ItemFactory
    {
        public static Item Create(ItemData data)
        {
            Item item = new Item(
                data.Id,
                data.Name,
                data.Description,
                data.UseText,
                data.Event?.Id
            );

            return item;
        }
    }
}
