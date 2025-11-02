namespace WorldOfZuul.Item
{

    public class Item
    {
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }


        public Item(string itemName, string itemDesc)
        {
            ItemName = itemName;
            ItemDesc = itemDesc;
        }


        public string Inspect()
        {
            return ItemDesc;
        }















    }
}
