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
        public Dictionary<string, InteractiveObject> InteractiveObjects { get; } = new();
        public RoomState State { get; } = new();

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
            // Try exact key first
            if (Exits.TryGetValue(id, out var exit))
            {
                return exit;
            }
            // Fallback: case-insensitive key match
            foreach (var kvp in Exits)
            {
                if (string.Equals(kvp.Key, id, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Value;
                }
            }
            return null;
        }



        /*
        * Returns an item by its ID or by name (case-insensitive).
        */
        public Item? GetItem(string id)
        {
            // First try exact ID match
            if (Items.TryGetValue(id, out var item))
            {
                return item;
            }

            // If no ID match, search by name (case-insensitive)
            foreach (var kvp in Items)
            {
                if (kvp.Value.Name.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Value;
                }
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
            if (exit == null) return;
            Exits[exit.Id] = exit;
        }



        /*
        * Sets a new item for the room.
        */
        public void SetItem(Item item)
        {
            if (item == null) return;
            Items[item.Id] = item;
        }



        /*
        * Sets a new npc for the room.
        */
        public void SetNpc(Npc npc)
        {
            if (npc == null) return;
            Npcs[npc.Id] = npc;
        }

        /*
        * Handles a player action on an interactive object in this room.
        * Returns the response text and applies any side effects to the room state.
        */
        public string HandleInteractiveAction(string objectId, string verb)
        {
            if (!InteractiveObjects.TryGetValue(objectId, out var obj))
            {
                // Try case-insensitive match on object IDs
                foreach (var kvp in InteractiveObjects)
                {
                    if (string.Equals(kvp.Key, objectId, StringComparison.OrdinalIgnoreCase))
                    {
                        obj = kvp.Value;
                        break;
                    }
                }
            }

            if (obj == null)
                return $"There's no '{objectId}' here.";

            var (response, sideEffect) = obj.Execute(State, verb);
            sideEffect?.Invoke(State);

            return response;
        }
    }
}
