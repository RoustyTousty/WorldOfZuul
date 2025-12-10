using WorldOfZuul.World;
using WorldOfZuul.Items;

namespace WorldOfZuul.Entities
{
    public class Player
    {
        public string? Name { get; private set; }
        public Location CurrentLocation { get; private set; }
        public Room CurrentRoom { get; private set; }
        public Room? PreviousRoom { get; private set; }

        public Inventory Inventory { get; } = new();

        private const string DEFAULT_PLAYER_NAME = "Detective Conan";

        public Player(Location startingLocation)
        {
            CurrentLocation = startingLocation;
            CurrentRoom = startingLocation.GetRoom(startingLocation.StartingRoomId)!;
        }



        /*
        * Attempts to move the player through a specified exit.
        * Checks if the exit exists, the room exists and if the exit is locked.
        * Attempts to unlock the exit using inventory items if it is locked.
        */
        public bool MoveToRoom(string? exitId)
        {
            if (string.IsNullOrWhiteSpace(exitId))
            {
                Console.WriteLine("Move where?");
                return false;
            }

            Exit? exit = CurrentRoom.GetExit(exitId);

            if (exit == null)
            {
                Console.WriteLine($"There is no exit called '{exitId}'.");
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
            PrintRoom();
            return true;
        }



        /*
        * Moves the player back to the last room.
        */
        public bool BackToRoom()
        {
            if (PreviousRoom == null)
            {
                Console.WriteLine("There’s nowhere to go back to.");
                return false;
            }

            Room temp = CurrentRoom;
            CurrentRoom = PreviousRoom;
            PreviousRoom = temp;

            Console.WriteLine($"You return to {CurrentRoom.Name}.");
            PrintRoom();
            return true;
        }
        



        /*
        * Moves the player to a new locations starting room.
        */
        public bool MoveToLocation(string? locationName, Map map)
        {
            if (string.IsNullOrWhiteSpace(locationName))
            {
                Console.WriteLine("Travel where?");
                return false;
            }

            Location? newLocation = map.GetLocation(locationName);

            if (newLocation == null)
            {
                Console.WriteLine($"There is no location called '{locationName}'.");
                return false;
            }

            CurrentLocation = newLocation;
            PreviousRoom = null;
            CurrentRoom = newLocation.GetRoom(newLocation.StartingRoomId)!;

            Console.WriteLine($"You travel to {newLocation.Name} and arrive at {CurrentRoom.Name}.");
            PrintRoom();
            return true;
        }



        /*
        * Attempts to take an item from the current room and add it to the player's inventory.
        */
        public void TryTakeItem(string itemName)
        {
            var item = CurrentRoom.GetItem(itemName);
            if (item == null)
            {
                Console.WriteLine($"There is no item named {itemName} here.");
                return;
            }

            if (!item.CanPickUp)
            {
                Console.WriteLine($"You can't pick up the {item.Name}.");
                return;
            }

            Inventory.AddItem(item);
            //TODO: Remove item from room
            Console.WriteLine($"You picked up the {item.Name}.");
        }




        public void TryUseItem(string itemName)

        {
            var item = Inventory.GetItem(itemName);    //supposed to check for and get the item from inventory
            if (item == null)
            {
                Console.WriteLine($"You don't have that item in your inventory.");
                return;
            }

            item.Use();
        }

        
        public void TryDropItem(string itemName)

        {
            var item = Inventory.GetItem(itemName);
            if (item == null)
            {
                Console.WriteLine($"You don't have that item in your inventory.");
                return;
            }

            Inventory.RemoveItem(item);
            //TODO: When dropping the item, it needs to put the item into the room it was dropped in.
            item.Drop();
    
        }
    public void PrintInventory() // creates a public empty method that displays the items in the inventory
        {
            if (Inventory.items.Length == 0)
            {
                Console.WriteLine("Your inventory is empty.");
                return;
            }
            else
            {
            Console.WriteLine("Inventory:"); // Prints the header "Inventory:"
            foreach (Item item in Inventory.items) // Loops through each item in the array(items)
            {
                if (item == Inventory.items[0])
                {
                    Console.WriteLine(item); // Prints the first item without extra line
                }
                else
                Console.WriteLine(" - " + item);// Prints each intem with a dash in front
            }
            }}

        public void TryInspectItem(string itemName)

        {
            var item = Inventory.GetItem(itemName);
            if (item == null)
            {
                
                Console.WriteLine($"You don't have that item in your inventory.");
                return;
                
            }

            item.Inspect();
        }










        /*
        * Attempts to unlock an exit using the items inside players inventory.
        */
        private bool TryUnlockExit(Exit exit)
        {
            if (string.IsNullOrEmpty(exit.KeyItemId))
            {
                Console.WriteLine("This exit cannot be unlocked.");
                return false;
            }

            if (Inventory.HasItemWithId(exit.KeyItemId))
            {
                exit.Unlock();
                Console.WriteLine($"You unlocked the {exit.Name}!");
                return true;
            }
            Console.WriteLine("You might need a key or an item to unlock it.");
            return false;
            
        }


        /*
        * Attempts to talk to an NPC in the current room.
        */
        public void TryTalkToNpc(string npcName)
        {
            /*
            * Look for an npc with a matching or close to matching name to the input.
            */
            var npc = CurrentRoom.Npcs.Values.FirstOrDefault(n => n.Name.Contains(npcName, StringComparison.OrdinalIgnoreCase));

            if (npc == null)
            {
                Console.WriteLine($"There is no one named {npcName} here.");
                return;
            }

            npc.StartDialogue();
        }



        /*        
        * Prompts the player to enter their name. If no name is entered, a default name is assigned.
        */
        public void PromptPlayerName()
        {
            Console.WriteLine("Enter your name!");
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Name = DEFAULT_PLAYER_NAME;
                Console.WriteLine($"No name entered. You will be known as {Name}!");
            }
            else
            {
                Name = input;
            }
            Console.WriteLine($"Welcome, {Name}!");
        }



        /*
        * Prints the description of the current room, its exits, npcs and any items inside.
        */
        public void PrintRoom()
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
                    string lockInfo = exit.Value.IsLocked ? "(locked)" : "";
                    Console.WriteLine($" - {exit.Key} {lockInfo}");
                }
            }
            else
            {
                Console.WriteLine("There are no visible exits.");
            }

            if (CurrentRoom.Items.Count > 0)
            {
                Console.WriteLine("\nYou see:");
                foreach (var item in CurrentRoom.Items.Values)
                {
                    Console.WriteLine($" - {item.Name} ({item.Description})");
                }
            }

            if (CurrentRoom.Npcs.Count > 0)
            {
                Console.WriteLine("\nNpcs:");
                foreach (var npc in CurrentRoom.Npcs.Values)
                {
                    Console.WriteLine($" - {npc.Name} ({npc.Description})");
                }
            }

            Console.WriteLine();
        }


        /*        
        * Prints the welcome message.
        */
        public void PrintWelcome()
        {
            Console.WriteLine();
            Console.WriteLine($"Welcome {Name} to the Clean Hands!");
            Console.WriteLine("An investigative text game inspired by the Tangentopoli scandal.");                                                                                                 
            Console.WriteLine();
            PrintHelp();
            Console.WriteLine();
        }



        /*
        * Prints the help message and lists available commands and their usage.
        */
        public void PrintHelp()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine(" - move [exitName]");
            Console.WriteLine(" - back");
            Console.WriteLine(" - look");
            Console.WriteLine(" - inventory");
            Console.WriteLine(" - help");
            Console.WriteLine(" - quit");
        }



        /*
        * Prints empty lines to the console for better readability.
        */
        public void PrintEmptySpace(int lines = 1)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.WriteLine();
            }
        }
    }
}
