using WorldOfZuul.World;
using WorldOfZuul.Items;

namespace WorldOfZuul.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public Location CurrentLocation { get; private set; }
        public Room CurrentRoom { get; private set; }
        public Room? PreviousRoom { get; private set; }

        public Inventory Inventory { get; } = new();

        public Player(string name, Location startingLocation)
        {
            Name = name;
            CurrentLocation = startingLocation;
            CurrentRoom = startingLocation.StartingRoom;
        }



        /*
        * Attempts to move the player through a specified exit.
        * Provides in-game feedback and returns true if successful.
        */
        public bool Move(string exitName)
        {
            if (string.IsNullOrWhiteSpace(exitName))
            {
                Console.WriteLine("Move where?");
                return false;
            }

            Exit? exit = CurrentRoom.GetExit(exitName);

            if (exit == null)
            {
                Console.WriteLine($"There is no exit called '{exitName}'.");
                return false;
            }

            if (exit.IsLocked)
            {
                Console.WriteLine($"The {exit.Name} is locked.");

                if (TryUnlockExit(exit))
                {
                    Console.WriteLine($"You unlocked the {exit.Name}!");
                }
                else
                {
                    Console.WriteLine("You might need a key or an item to unlock it.");
                    return false;
                }
            }

            if (exit.TargetRoom == null)
            {
                Console.WriteLine("That path seems to lead nowhere...");
                return false;
            }

            PreviousRoom = CurrentRoom;
            CurrentRoom = exit.TargetRoom;

            Console.WriteLine($"You enter {CurrentRoom.Name}.");
            Look();
            return true;
        }



        /*
        * Tries to unlock an exit using the items inside players inventory.
        */
        private bool TryUnlockExit(Exit exit)
        {
            List<Item> items = Inventory.GetItems();
            foreach (var item in items)
            {
                if (item.Id == exit.KeyItemId)
                {
                    exit.Unlock();
                    return true;
                }
            }
            return false;
        }



        /*
        * Moves the player to a new locations starting room.
        */
        public bool MoveToLocation(Location newLocation)
        {
            if (newLocation == null)
                return false;

            CurrentLocation = newLocation;
            PreviousRoom = null;
            CurrentRoom = newLocation.StartingRoom;

            Console.WriteLine($"You travel to {newLocation.Name} and arrive at {CurrentRoom.Name}.");
            Look();
            return true;
        }



        /*
        * Moves the player back to the last room.
        */
        public bool GoBack()
        {
            if (PreviousRoom == null)
            {
                Console.WriteLine("There’s nowhere to go back to.");
                return false;
            }

            var temp = CurrentRoom;
            CurrentRoom = PreviousRoom;
            PreviousRoom = temp;

            Console.WriteLine($"You return to {CurrentRoom.Name}.");
            Look();
            return true;
        }



        /*
        * Prints the description of the current room, its exits, and any items inside.
        */
        public void Look()
        {
            Console.WriteLine();
            Console.WriteLine($"--- {CurrentRoom.Name} ---");
            Console.WriteLine(CurrentRoom.Description);
            Console.WriteLine();

            if (CurrentRoom.Exits.Count > 0)
            {
                Console.WriteLine("Exits:");
                foreach (var exit in CurrentRoom.Exits)
                {
                    string lockInfo = exit.Value.IsLocked ? " (locked)" : "";
                    Console.WriteLine($" - {exit.Key}{lockInfo}");
                }
            }
            else
            {
                Console.WriteLine("There are no visible exits.");
            }

            if (CurrentRoom.Items.Count > 0)
            {
                Console.WriteLine("\nYou see:");
                foreach (var item in CurrentRoom.Items)
                {
                    Console.WriteLine($" - {item.Name}");
                }
            }

            Console.WriteLine();
        }



        /*
        * Prints a list of items in the player's inventory.
        */
        public void InventoryList()
        {
            List<Item> items = Inventory.GetItems();
            if (items.Count == 0)
            {
                Console.WriteLine("Your inventory is empty.");
                return;
            }

            Console.WriteLine("You are carrying:");
            foreach (var item in items)
            {
                Console.WriteLine($" - {item.Name}");
            }
        }
    }
}
