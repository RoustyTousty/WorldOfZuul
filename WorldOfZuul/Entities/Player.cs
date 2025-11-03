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

        private const string DEFAULT_PLAYER_NAME = "Bob the great adventurer";

        public Player(Location startingLocation)
        {
            CurrentLocation = startingLocation;
            CurrentRoom = startingLocation.StartingRoom;
        }



        /*
        * Attempts to move the player through a specified exit.
        * Checks if the exit exists, the room exists and if the exit is locked.
        * Attempts to unlock the exit using inventory items if it is locked.
        */
        public bool MoveToRoom(string? exitName)
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
        public bool MoveToLocation(Location newLocation)
        {
            if (newLocation == null)
                return false;

            CurrentLocation = newLocation;
            PreviousRoom = null;
            CurrentRoom = newLocation.StartingRoom;

            Console.WriteLine($"You travel to {newLocation.Name} and arrive at {CurrentRoom.Name}.");
            PrintRoom();
            return true;
        }



        /*
        * Attempts to unlock an exit using the items inside players inventory.
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
                Console.WriteLine($"Welcome, {Name}!");
            }
        }



        /*
        * Prints the description of the current room, its exits, and any items inside.
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
                    Console.WriteLine($" - {item.Name} ({item.Description})");
                }
            }

            Console.WriteLine();
        }



        /*
        * Prints a list of items in the player's inventory.
        */
        public void PrintInventory()
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



        /*        
        * Prints the welcome message.
        */
        public void PrintWelcome()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to the World of Zuul!");
            Console.WriteLine("World of Zuul is a new, incredibly boring adventure game.");
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
            Console.WriteLine(" - inevntory");
            Console.WriteLine(" - help");
            Console.WriteLine(" - quit");
        }
    }
}
