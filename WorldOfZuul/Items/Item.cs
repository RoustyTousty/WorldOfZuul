namespace WorldOfZuul.Item
{
    public class Item
    {
        public string Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }


        public Item(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }


        public string Inspect()
        {
            return Description;
        }















    }
}
