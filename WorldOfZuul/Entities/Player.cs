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
            Console.WriteLine($"Starting location set to {startingLocation}.");
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

            // Special check: Block exit from ENI to City until all evidence is collected
            if (CurrentRoom.Id == "ENI" && exit.Id == "City")
            {
                bool hasFolder = Inventory.HasItemWithId("folder");
                bool hasEnvelope = Inventory.HasItemWithId("envelope");
                bool hasDocuments = Inventory.HasItemWithId("documents");
                bool hasEvidence = Inventory.HasItemWithId("evidence");

                if (!hasFolder || !hasEnvelope || !hasDocuments || !hasEvidence)
                {
                    Console.WriteLine("You can't leave yet. You need to gather all the evidence first:\n");
                    if (!hasFolder) Console.WriteLine(" - Red folder with wire transfers");
                    if (!hasEnvelope) Console.WriteLine(" - Envelope from the center drawer");
                    if (!hasDocuments) Console.WriteLine(" - Documents from the safe");
                    if (!hasEvidence) Console.WriteLine(" - Evidence from the hidden compartment");
                    Console.WriteLine("\nYour investigation isn't complete. Keep searching the office.");
                    return false;
                }
                else
                {
                    // Player has all evidence - process completion
                    if (!CurrentRoom.State.GetFlag("eni_investigation_complete"))
                    {
                        // Remove evidence items from inventory
                        var folderItem = Inventory.GetItem("folder");
                        var envelopeItem = Inventory.GetItem("envelope");
                        var documentsItem = Inventory.GetItem("documents");
                        var evidenceItem = Inventory.GetItem("evidence");

                        if (folderItem != null) Inventory.RemoveItem(folderItem);
                        if (envelopeItem != null) Inventory.RemoveItem(envelopeItem);
                        if (documentsItem != null) Inventory.RemoveItem(documentsItem);
                        if (evidenceItem != null) Inventory.RemoveItem(evidenceItem);

                        // Award completion medal
                        var medal = new Item(
                            "eni_medal",
                            "Medal of Investigation Excellence",
                            "A prestigious medal awarded for successfully completing the ENI Executive Office investigation. " +
                            "You gathered all critical evidence linking ENI to the Tangentopoli corruption scandal."
                        );
                        Inventory.AddItem(medal);

                        // Mark investigation as complete
                        CurrentRoom.State.SetFlag("eni_investigation_complete");

                        // Display completion message
                        Console.Clear();
                        Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║                   INVESTIGATION COMPLETE!                        ║");
                        Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝\n");
                        Console.WriteLine("You carefully secure all the evidence in your briefcase:");
                        Console.WriteLine(" ✓ Red folder with wire transfer records");
                        Console.WriteLine(" ✓ Sealed envelope from the desk");
                        Console.WriteLine(" ✓ Documents from the safe");
                        Console.WriteLine(" ✓ Hidden compartment evidence\n");
                        Console.WriteLine("This evidence will be crucial in exposing the corruption network.");
                        Console.WriteLine("Your superiors will be impressed with your thorough investigation.\n");
                        Console.WriteLine("🏅 You have been awarded: Medal of Investigation Excellence\n");
                        Console.WriteLine("Continue fighting against corruption and bringing justice to light through exploring the other rooms of the city.\n");
                        Console.WriteLine("Continue your journey by typing 'look'.");
                        Console.ReadLine();
                    }
                }
            }

            //-Edris
            // Special check: Block exit from Office 01  until all evidence is collected
            if (CurrentRoom.Id == "Office 01" && exit.Id == "Political Offices")
            {
                bool hasRolexBox = Inventory.HasItemWithId("rolex_box");
                bool hasNotebook = Inventory.HasItemWithId("notebook");
                bool hasNote = Inventory.HasItemWithId("note");

                if (!hasRolexBox || !hasNotebook || !hasNote)
                {
                    Console.WriteLine("You can't leave yet. You need to gather all the evidence first:\n");
                    if (!hasRolexBox) Console.WriteLine(" - Rolex box with certificate");
                    if (!hasNotebook) Console.WriteLine(" - Meeting notes notebook");
                    if (!hasNote) Console.WriteLine(" - Handwritten note on letterhead");
                    Console.WriteLine("\nYour investigation isn't complete. Keep searching the office.");
                    return false;
                }
                else
                {
                    // Player has all evidence - process completion
                    if (!CurrentRoom.State.GetFlag("office01_investigation_complete"))
                    {
                        // Remove evidence items from inventory
                        var rolexBoxItem = Inventory.GetItem("rolex_box");
                        var notebookItem = Inventory.GetItem("notebook");
                        var noteItem = Inventory.GetItem("note");

                        if (rolexBoxItem != null) Inventory.RemoveItem(rolexBoxItem);
                        if (notebookItem != null) Inventory.RemoveItem(notebookItem);
                        if (noteItem != null) Inventory.RemoveItem(noteItem);

                        // Award completion medal
                        var medal = new Item(
                            "housing_medal",
                            "Medal of Municipal Investigation",
                            "A medal awarded for undercovering corruption evidence in the department of housing and urban development. " +
                            "You exposed the connection between edilcoop and suspicious municipal contracts."
                        );
                        Inventory.AddItem(medal);

                        // Mark investigation as complete
                        CurrentRoom.State.SetFlag("office01_investigation_complete");

                        // Display completion message
                        Console.Clear();
                        Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║                   HOUSING DEPARTMENT INVESTIGATION COMPLETE!                        ║");
                        Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝\n");
                        Console.WriteLine("You carefully secure all the evidence in your briefcase:");
                        Console.WriteLine(" ✓ Empty Rolex Box with Edilcoop certificate");
                        Console.WriteLine(" ✓ Meeting notes linking politicians to construction firms");
                        Console.WriteLine(" ✓ Handwritten note about festival donations and permits");
                        Console.WriteLine("While no single document proves guilt, together they paint a troubling picture.");
                        Console.WriteLine("Your superiors will be impressed with your thorough investigation.\n");
                        Console.WriteLine("You have been awarded: Medal of Municipal Investigation\n");
                        Console.WriteLine("Continue fighting corruption by investigating other rooms in the city.\n");
                        Console.WriteLine("Continue your journey by typing 'look'.");
                        Console.ReadLine();
                    }
                }
            }

            //-Edris

            if (exit.IsLocked)
            {
                Console.WriteLine($"The {exit.Name} is locked.");
                return false;
            }

            // 🔹 CASE 1: Exit leads to another LOCATION
            if (exit.TargetLocation != null)
            {
                CurrentLocation = exit.TargetLocation;
                PreviousRoom = null;
                CurrentRoom = CurrentLocation.GetRoom(CurrentLocation.StartingRoomId)!;

                Console.WriteLine($"You travel to {CurrentLocation.Name}.");
                PrintRoom();
                return true;
            }

            // 🔹 CASE 2: Exit leads to another ROOM
            if (exit.TargetRoom != null)
            {
                PreviousRoom = CurrentRoom;
                CurrentRoom = exit.TargetRoom;

                Console.WriteLine($"You enter {CurrentRoom.Name}.");
                PrintRoom();
                return true;
            }

            Console.WriteLine(exit.TargetLocation);
            Console.WriteLine("That path seems to lead nowhere...");
            return false;
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
        * Attempts to take an item from the current room and add it to the player's inventory.
        */
        public void TryTakeItem(string itemName)
        {
            Item? item = CurrentRoom.GetItem(itemName);
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

            // Add to inventory
            Inventory.AddItem(item);
            
            // Remove from the room so it can't be taken again
            if (CurrentRoom.Items.ContainsKey(item.Id))
            {
                CurrentRoom.Items.Remove(item.Id);
            }
            else
            {
                // Fallback: remove by reference if id key differs
                var kvp = CurrentRoom.Items.FirstOrDefault(k => ReferenceEquals(k.Value, item));
                if (!string.IsNullOrEmpty(kvp.Key))
                {
                    CurrentRoom.Items.Remove(kvp.Key);
                }
            }

            // Special narrative for compartment evidence bundle
            if (item.Id == "evidence")
            {
                Console.WriteLine("You carefully remove the briefcase and ledger from the compartment and place them in your bag.\n"
                                  + "Your heart is pounding. If anyone discovers these are missing, they'll know someone was here.\n"
                                  + "These documents link ENI, the Vatican Bank (IOR), and offshore accounts in Switzerland. This is THE evidence.");
                return;
            }

            // Special narrative for the red folder
            if (item.Id == "folder")
            {
                // Preserve flag that was previously set in the interactive object
                CurrentRoom.State.SetFlag("evidence_taken");
                Console.WriteLine("You take the red folder and tuck it into your bag. You have a gut feeling someone will notice it's missing...");
                return;
            }

            // Special narrative for the safe documents bundle
            if (item.Id == "documents")
            {
                Console.WriteLine("You collect the entire evidence bundle from the safe: documents, bank statement, and the sealed photograph."
                                  + "\nIf anyone checks the safe, they'll know this trove is gone.");
                return;
            }

            // Friendly grammar: avoid "the" if name already starts with an article
            string name = item.Name;
            bool startsWithArticle = name.StartsWith("a ", StringComparison.OrdinalIgnoreCase)
                                     || name.StartsWith("an ", StringComparison.OrdinalIgnoreCase)
                                     || name.StartsWith("the ", StringComparison.OrdinalIgnoreCase);
            string printable = startsWithArticle ? name : $"the {name}";

            Console.WriteLine($"You picked up {printable}.");
        }



        /*
        * Attempts to use an item from the player's inventory.
        */
        public void TryUseItem(string itemName)
        {
            Item? item = Inventory.GetItem(itemName);
            if (item == null)
            {
                Console.WriteLine($"You don't have that item in your inventory.");
                return;
            }

            item.Use();
        }


        /*
        * Attempts to drop an item from the player's inventory into the current room.
        */
        public void TryDropItem(string itemName)
        {
            Item? item = Inventory.GetItem(itemName);
            if (item == null)
            {
                Console.WriteLine($"You don't have that item in your inventory.");
                return;
            }

            Inventory.RemoveItem(item);
            CurrentRoom.SetItem(item);

            item.Drop();
    
        }
        public void PrintInventory()
        {
            Console.Clear();
            
            int boxHeight = DrawCommandBox();
            Console.SetCursorPosition(0, boxHeight + 1);

            if (Inventory.items.Count == 0)
            {
                Console.WriteLine("Your inventory is empty.");
                return;
            }

            Console.WriteLine("Inventory:");
            foreach (Item item in Inventory.items)
            {
                Console.WriteLine($" - {item.Name}");
            }
        }



        /*
        * Attempts to drop an item from the player's inventory into the current room.
        */
        public void TryInspectItem(string itemName)
        {
            Item? item = Inventory.GetItem(itemName);
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
            Console.WriteLine("You might need a key or an item to unlock it.");
            exit.Unlock();
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
            Npc? npc = CurrentRoom.Npcs.Values.FirstOrDefault(n => n.Name.Contains(npcName, StringComparison.OrdinalIgnoreCase));

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
        *Prints the Welcome message when the game starts.
        */
        public void PrintWelcome()
        {

            Console.Clear();      
            int boxHeight = DrawCommandBox();  
            Console.SetCursorPosition(0, boxHeight + 1);    
            Console.WriteLine();
            Console.WriteLine($"Welcome {Name} to Clean Hands!");
            Console.WriteLine("An investigative text-based game inspired by the Tangentopoli scandal.");                                                                                                 
            Console.WriteLine("Start by taking a \"look\" around your current location.");  
            Console.WriteLine();
        }

        /*
        * Prints the description of the current room, its exits, npcs and any items inside.
        * Displays an ASCII command box in the top-right corner.
        */

        public void PrintRoom()
        {
            Console.Clear();
            
            // Draw command box and get its height
            int boxHeight = DrawCommandBox();

            // Reset to left side for room description
            Console.SetCursorPosition(0, boxHeight + 1);
            
            Console.WriteLine();

            // LOCATION HEADER
            Console.WriteLine($"=== {CurrentLocation.Name} ===");
            Console.WriteLine(CurrentLocation.Description);
            Console.WriteLine();

            // ROOM HEADER
            Console.WriteLine($"--- {CurrentRoom.Name} ---");
            Console.WriteLine(CurrentRoom.Description);
            Console.WriteLine();

            // EXITS
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
                Console.WriteLine("Exits:");
                Console.WriteLine(" - None");
            }

            // ITEMS
            Console.WriteLine();
            Console.WriteLine("Items:");
            if (CurrentRoom.Items.Count > 0)
            {
                foreach (var item in CurrentRoom.Items.Values)
                {
                    Console.WriteLine($" - {item.Name}: {item.Description}");
                }
            }
            else
            {
                Console.WriteLine(" - None");
            }

            // NPCS
            Console.WriteLine();
            Console.WriteLine("NPCs:");
            if (CurrentRoom.Npcs.Count > 0)
            {
                foreach (var npc in CurrentRoom.Npcs.Values)
                {
                    Console.WriteLine($" - {npc.Name}: {npc.Description}");
                }
            }
            else
            {
                Console.WriteLine(" - None");
            }

            Console.WriteLine();
        }

        /*       
        * Draws the command reference box in the top-right corner of the screen.
        * Returns the number of lines used by the box.
        */           
        private int DrawCommandBox()
        {
            string[] commandBox = new string[]
            {
                "╔═══════════════════════╗",
                "║   AVAILABLE COMMANDS  ║",
                "╠═══════════════════════╣",
                "║ move [ExitName]       ║",
                "║ back                  ║",
                "║ look                  ║",
                "║ take [ItemName]       ║",
                "║ inspect [Object]      ║",
                "║ open [Object]         ║",
                "║ inventory             ║",
                "║ help                  ║",
                "║ quit                  ║",
                "╚═══════════════════════╝"
            };

            // Print command box in top-right corner (console width - box width)
            int boxWidth = 25;
            int rightPos = Math.Max(0, Console.WindowWidth - boxWidth - 2);
            for (int i = 0; i < commandBox.Length; i++)
            {
                int row = Console.WindowTop + 1 + i; // one-line margin to avoid top cropping
                Console.SetCursorPosition(rightPos, row);
                Console.Write(commandBox[i]);
            }

            return commandBox.Length;
        }

        /*
        * Prints the help message and lists available commands and their usage.
        */
        public void PrintHelp()
        {
            Console.Clear();
            
            // Draw command box and get its height
            int boxHeight = DrawCommandBox();
            
            // Reset to left side for help content
            Console.SetCursorPosition(0, boxHeight + 1);
            
            Console.WriteLine("Commands:");
            Console.WriteLine(" - move [exitName] - Move through an exit");
            Console.WriteLine(" - back - Return to previous room");
            Console.WriteLine(" - look - Look around the current room");
            Console.WriteLine(" - take [itemName] - Pick up an item");
            Console.WriteLine(" - inspect [object] - Examine an object or item");
            Console.WriteLine(" - open [object] - Open drawers, doors, etc.");
            Console.WriteLine(" - inventory - View your inventory");
            Console.WriteLine(" - help - Show this help message");
            Console.WriteLine(" - quit - Exit the game");
        }

        // Draw the command box without clearing existing content
        public void ShowCommandBox()
        {
            int savedLeft = Console.CursorLeft;
            int savedTop = Console.CursorTop;
            DrawCommandBox();
            // Restore cursor so the input prompt stays after the last output
            Console.SetCursorPosition(savedLeft, savedTop);
        }

        // Draws the command box and moves the cursor to the content area below it
        public void BeginContentArea()
        {
            int boxHeight = DrawCommandBox();
            Console.SetCursorPosition(0, Console.WindowTop + boxHeight + 1);
        }

        // Clears the screen, redraws the command box, and positions cursor in the content area
        public void ClearAndBeginContentArea()
        {
            Console.Clear();
            int boxHeight = DrawCommandBox();
            Console.SetCursorPosition(0, Console.WindowTop + boxHeight + 1);
        }

        // Ensure there's space at the bottom for the status line and prompt
        public void EnsurePromptArea(int reservedLines = 2)
        {
            int bottom = Console.WindowTop + Console.WindowHeight;
            int targetTop = bottom - reservedLines;
            if (Console.CursorTop < targetTop)
            {
                int needed = targetTop - Console.CursorTop;
                for (int i = 0; i < needed; i++)
                {
                    Console.WriteLine();
                }
            }
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

        /*
        * Displays a response message with the command box visible.
        * Used for interactive object responses (inspect, open, etc.)
        */
        public void PrintResponse(string message)
        {
            Console.Clear();
            
            // Draw command box and get its height
            int boxHeight = DrawCommandBox();
            
            // Reset to left side for response content
            Console.SetCursorPosition(0, boxHeight + 1);
            
            Console.WriteLine(message);
            Console.WriteLine();
        }
    }
}
