namespace WorldOfZuul.Item
{
    public abstract class Item
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected Item(string id, string name, string description = "")
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public virtual string Examine() => Description;
    }
}
