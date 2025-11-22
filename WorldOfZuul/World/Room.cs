using WorldOfZuul.Items;
using WorldOfZuul.Entities;

namespace WorldOfZuul.World
{
    /*
    * Room class representing a location in the game world.
    */
    public class Room
    {
        public string Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Dictionary<string, Exit> Exits { get; } = new();
        public Dictionary<string, Item> Items { get; } = new();
        public Dictionary<string, Npc> Npcs { get; } = new();

        public Room(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }



        /*
        * Returns an exit by its ID.
        */
        public Exit? GetExit(string id)
        {
            if (Exits.TryGetValue(id, out var exit))
            {
                return exit;
            }
            return null;
        }



        /*
        * Returns an item by its ID.
        */
        public Item? GetItem(string id)
        {
            if (Items.TryGetValue(id, out var item))
            {
                return item;
            }
            return null;
        }



        /*
        * Returns an npc by its ID.
        */
        public Npc? GetNpc(string id)
        {
            if (Npcs.TryGetValue(id, out var npc))
            {
                return npc;
            }
            return null;
        }



        /*
        * Sets a new exit for the room.
        */
        public void SetExit(Exit exit)
        {
            Exits.Add(exit.Id, exit);
        }



        /*
        * Sets a new item for the room.
        */
        public void SetItem(Item item)
        {
            Items.Add(item.Id, item);
        }



        /*
        * Sets a new npc for the room.
        */
        public void SetNpc(Npc npc)
        {
            Npcs.Add(npc.Id, npc);
        }



        /*
        * Remove Exit from the room.
        */
        public void RemoveExit(Exit exit)
        {
            Exits.Remove(exit.Id);
        }



        /*
        * Remove item from the room.
        */
        public void RemoveItem(Item item)
        {
            Items.Remove(item.Id);
        }



        /*
        * Remove Npc from the room.
        */
        public void RemoveNpc(Npc npc)
        {
            Npcs.Remove(npc.Id);
        }
    }
}
