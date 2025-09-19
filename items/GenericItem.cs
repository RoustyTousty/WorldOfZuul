namespace WorldOfZuul
{
    public class GenericItem : Item
    {
        public GenericItem(string id, string name, string description) : base(id, name, description) {}

        public override void Interact(Game game)
        {
            base.Interact(game);
        }
    }
}