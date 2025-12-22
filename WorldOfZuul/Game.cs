using WorldOfZuul.World;
using WorldOfZuul.Entities;
using WorldOfZuul.Commands;
using WorldOfZuul.Events;

namespace WorldOfZuul
{
    public class Game
    {
        private Player player;
        private Map map;
        public CaseState Case { get; } = new();

        public Game()
        {
            /*
            * Build the game world.
            */
            Builder builder = new Builder();
            map = builder.BuildMapFromJSON();
 /*
            * Add the Montedison Executive Office room (interactive demonstration).
            */
             Room montedisonOffice = MontedisonRoomBuilder.BuildMontedisonOffice();
             // Replace the JSON Montedison room with the interactive version
              var locationWithMontedison = map.Locations.Values
        .FirstOrDefault(loc => loc.Rooms.ContainsKey("Montedison"));
        if (locationWithMontedison != null)
    {
        Room oldRoom = locationWithMontedison.GetRoom("Montedison")!;
        locationWithMontedison.SetRoom(montedisonOffice);
 // Update all exits in the map that were pointing to the old Montedison room to point to the new one
        foreach (var location in map.Locations.Values)
        {
            foreach (var room in location.Rooms.Values)
            {
                foreach (var exit in room.Exits.Values)
                {
                    if (exit.TargetRoom == oldRoom)
                    {
                        exit.TargetRoom = montedisonOffice;
                    }
                }
            }
        }
    }
            /*
            * Add the ENI Executive Office room (interactive demonstration).
            */
            Room eniOffice = ENIRoomBuilder.BuildENIExecutiveOffice();

            // Replace the JSON ENI room with the interactive version
            var courtHouseLocation = map.GetLocation("Court House");
            if (courtHouseLocation != null && courtHouseLocation.Rooms.ContainsKey("ENI"))
            {
                Room oldEniRoom = courtHouseLocation.GetRoom("ENI")!;
                courtHouseLocation.SetRoom(eniOffice);
                
                // Update all exits in the map that were pointing to the old ENI room to point to the new one
                foreach (var location in map.Locations.Values)
                {
                    foreach (var room in location.Rooms.Values)
                    {
                        foreach (var exit in room.Exits.Values)
                        {
                            if (exit.TargetRoom == oldEniRoom)
                            {
                                exit.TargetRoom = eniOffice;
                            }
                        }
                    }
                }
                
                // Set up the ENI room's exit back to City
                if (eniOffice.Exits.ContainsKey("City"))
                {
                    var cityRoom = courtHouseLocation.GetRoom("City");
                    if (cityRoom != null)
                    {
                        eniOffice.Exits["City"].TargetRoom = cityRoom;
                    }
                }
            }

            //---
          /*
            * Add the Department of Housing and Urban Development Office room (interactive demonstration).
            */
            Room HousingandUrbanDevelopment = Office1RoomBuilder.BuildPoliticalOffice01();

            // Replace the JSON Office 01 room with the interactive version
            if (courtHouseLocation != null && courtHouseLocation.Rooms.ContainsKey("Office 01"))
            {
                Room oldOffice1Room = courtHouseLocation.GetRoom("Office 01")!;
                courtHouseLocation.SetRoom(HousingandUrbanDevelopment);

                // Update all exits in the map that were pointing to the old Office 01 room to point to the new one
                foreach (var location in map.Locations.Values)
                {
                    foreach (var room in location.Rooms.Values)
                    {
                        foreach (var exit in room.Exits.Values)
                        {
                            if (exit.TargetRoom == oldOffice1Room)
                            {
                                exit.TargetRoom = HousingandUrbanDevelopment;
                            }
                        }
                    }
                }

                // Set up the Housing and Urban Development room's exit back to Political Offices
                if (HousingandUrbanDevelopment.Exits.ContainsKey("Political Offices"))
                {
                    var politicalOfficesRoom = courtHouseLocation.GetRoom("Political Offices");
                    if (politicalOfficesRoom != null)
                    {
                        HousingandUrbanDevelopment.Exits["Political Offices"].TargetRoom = politicalOfficesRoom;
                    }
                }
            }
           // ---

            /*
            * Initialize player and prompt for a name.
            */
            player = new Player(map.GetLocation(map.StartingLocationId)!);

            EventManager.Instance.Initialize(this, player, map);

            player.PrintEmptySpace(50);
            player.PromptPlayerName();
        }



        /*   
        * Main game cycle. Loops until end of the game.
        * Manages user input (Commands) and executes them.
        */
        public void Play()
        {
            player.PrintWelcome();
            Parser parser = new();

            bool continuePlaying = true;
            while (continuePlaying)
            {
                // Ensure the command box is always visible
                player!.ShowCommandBox();
                // Draw a status line with current room above the prompt
                int bottom = Console.WindowTop + Console.WindowHeight;
                int statusRow = bottom - 2;
                int promptRow = bottom - 1;
                // Clear status and prompt lines cleanly
                Console.SetCursorPosition(0, statusRow);
                Console.Write(new string(' ', Math.Max(0, Console.WindowWidth - 1)));
                Console.SetCursorPosition(0, statusRow);
                Console.Write($"Current room: {player.CurrentRoom.Name}");

                Console.SetCursorPosition(0, promptRow);
                Console.Write(new string(' ', Math.Max(0, Console.WindowWidth - 1))); // clear line without wrapping
                Console.SetCursorPosition(0, promptRow);
                Console.Write("> ");

                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Please enter a command.");
                    continue;
                }

                Command? command = parser.GetCommand(input);
                if (command == null)
                {
                    Console.WriteLine("I don't know that command.");
                    continue;
                }

                continuePlaying = HandleCommand(command);
                Console.WriteLine();
            }

            Console.WriteLine("Thank you for playing Clean Hands! Goodbye.");
        }
        


        /*
        * Executes the given commands.
        * Returns false if the game should end.
        */
        private bool HandleCommand(Command command)
        {
            if (player == null)
            {
                Console.WriteLine("Player not initialized.");
                return true;
            }

            // Clear screen and redraw command box for every command response
            player!.ClearAndBeginContentArea();

            switch (command.Name)
            {
                case "look":
                    player.PrintRoom();
                    break;


                case "inspect":
                    if (command.SecondWord == null)
                    {
                        player!.PrintResponse("Inspect what?");
                        break;
                    }

                    // First try to inspect an interactive object in the room
                    string inspectResponse = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, "inspect") ?? "";
                    if (!string.IsNullOrEmpty(inspectResponse) && !inspectResponse.Contains("You can't"))
                    {
                        player!.PrintResponse(inspectResponse);
                    }
                    else
                    {
                        // If no interactive object found, try inspecting an item in inventory
                        player?.TryInspectItem(command.SecondWord);
                    }
                    break;


                case "use":
                    if (command.SecondWord == null)
                    {
                        player!.PrintResponse("Use what?");
                        break;
                    }
                    player!.ClearAndBeginContentArea();
                    player!.TryUseItem(command.SecondWord);
                    break;


                case "take":
                    if (command.SecondWord == null)
                    {
                        player!.PrintResponse("Take what?");
                        break;
                    }
                    // Clear and draw the command box, then print take messages beneath it
                    player!.ClearAndBeginContentArea();
                    player!.TryTakeItem(command.SecondWord);
                    break;


                case "drop":
                    if (command.SecondWord == null)
                    {
                        player!.PrintResponse("Drop what?");
                        break;
                    }
                    player!.ClearAndBeginContentArea();
                    player!.TryDropItem(command.SecondWord);
                    break;


                case "read":
                    if (command.SecondWord == null)
                    {
                        player!.PrintResponse("Read what?");
                        break;
                    }

                    // Try to read an interactive object in the room
                    string readResponse = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, $"read {command.SecondWord}") ?? "";
                    if (!string.IsNullOrEmpty(readResponse) && readResponse != "You can't do that.")
                    {
                        player!.PrintResponse(readResponse);
                    }
                    else
                    {
                        player!.PrintResponse($"You can't read that.");
                    }
                    break;


                case "open":
                    if (command.SecondWord == null)
                    {
                        player!.PrintResponse("Open what?");
                        break;
                    }
                    
                    // Extract object id (first word) from potentially multi-word input like "safe 214"
                    string[] parts = command.SecondWord.Split(' ', 2);
                    string objectId = parts[0];
                    
                    if (objectId.Contains("drawer", StringComparison.OrdinalIgnoreCase))
                    {
                        objectId = "desk";
                    }

                    string openResponse = player?.CurrentRoom.HandleInteractiveAction(objectId, $"open {command.SecondWord}") ?? "";
                    if (!string.IsNullOrEmpty(openResponse) && openResponse != "You can't do that.")
                    {
                        player!.PrintResponse(openResponse);
                    }
                    else
                    {
                        player!.PrintResponse("You can't open that.");
                    }
                    break;


                case "answer":
                    string answerResponse = "";
                    if (command.SecondWord == null)
                    {
                        answerResponse = player?.CurrentRoom.HandleInteractiveAction("phone", "answer") ?? "";
                    }
                    else
                    {
                        answerResponse = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, "answer") ?? "";
                    }
                    
                    if (!string.IsNullOrEmpty(answerResponse) && answerResponse != "You can't do that.")
                    {
                        player!.PrintResponse(answerResponse);
                    }
                    else
                    {
                        player!.PrintResponse("You can't answer that.");
                    }
                    break;


                case "push":
                    if (command.SecondWord == null)
                    {
                        player!.PrintResponse("Push what?");
                        break;
                    }
                    
                    string pushResponse = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, $"push {command.SecondWord}") ?? "";
                    if (!string.IsNullOrEmpty(pushResponse) && pushResponse != "You can't do that.")
                    {
                        player!.PrintResponse(pushResponse);
                    }
                    else
                    {
                        player!.PrintResponse("You can't push that.");
                    }
                    break;


                case "inventory":
                    player!.PrintInventory();
                    break;


                case "back":
                    player!.BackToRoom();
                    break;


                case "move":
                    player!.MoveToRoom(command.SecondWord);
                    break;


                case "travel":
                    player!.MoveToLocation(command.SecondWord, map);
                    break;


                case "talk" or "talkto":
                    if (command.SecondWord == null)
                    {
                        player!.PrintResponse("Talk to who?");
                        break;
                    }
                    player!.ClearAndBeginContentArea();
                    player!.TryTalkToNpc(command.SecondWord);
                    break;

                
                case "conclude" or "ending":
                    return HandleEnding();


                case "quit":
                    return false;


                case "help":
                    player!.PrintHelp();
                    break;


                default:
                    player!.PrintResponse("I don't know that command.");
                    break;
            }
            // Next loop iteration will reposition the prompt at the bottom
            return true;
        }



        /*
        * Handles the game ending logic.
        */
        private bool HandleEnding()
        {
            if (Case.IsSolved)
            {
                Console.WriteLine("The case is already closed.");
                return true;
            }

            Console.WriteLine("You prepare to conclude the investigation...");

            if (Case.CanExposeTruth)
            {
                Console.WriteLine("\nYou release the evidence to the public.");
                Console.WriteLine("The city trembles as the truth comes out.");
                Console.WriteLine("Powerful figures fall. You made enemies.");
                Console.WriteLine("\nJustice was worth the cost.");

                Case.Solve();
                return false; // END GAME
            }
            else
            {
                Console.WriteLine("\nYour evidence is incomplete.");
                Console.WriteLine("Someone offers you a deal.");
                Console.WriteLine("The case disappears.");

                Case.Solve();
                return false; // END GAME (bad ending)
            }
        }
    }
}