using WorldOfZuul.World;
using WorldOfZuul.Entities;
using WorldOfZuul.Commands;
using WorldOfZuul.Items;

namespace WorldOfZuul
{
    public class Game
    {
        private Player player;
        private Map map;

        public Game()
        {
            /*
            * Build the game world.
            */
            Builder builder = new Builder();
            map = builder.BuildMapFromJSON();

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

            /*
            * Initialize player and prompt for a name.
            */
            player = new Player(map.GetLocation(map.StartingLocationId)!);
            player.PrintEmptySpace(50);
            player.PromptPlayerName();
            player.PrintWelcome();
        }



        /*   
        * Main game cycle. Loops until end of the game.
        * Manages user input (Commands) and executes them.
        */
        public void Play()
        {
            Parser parser = new();

            player?.PrintEmptySpace(50);
            player?.PrintWelcome(); 

            bool continuePlaying = true;
            while (continuePlaying)
            {
                Console.WriteLine($"Current room: {player?.CurrentRoom.Name}");
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

                player?.PrintEmptySpace(50);
                continuePlaying = HandleCommand(command);
                Console.WriteLine();
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }
        


        /*
        * Executes the given commands.
        * Returns false if the game should end.
        */
        private bool HandleCommand(Command command)
        {
            switch (command.Name)
            {
                case "look":
                    player.PrintRoom();
                    break;


                case "inspect":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Inspect what?");
                        break;
                    }
                    
                    // First try to inspect an interactive object in the room
                    string response = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, "inspect") ?? "";
                    if (!string.IsNullOrEmpty(response) && response != "You can't do that." && response != "You can't inspect that.")
                    {
                        Console.WriteLine(response);
                    }
                    else
                    {
                        // If no interactive object found, try inspecting an item in inventory
                        player.TryInspectItem(command.SecondWord);
                    }
                    break;


                case "use":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Use what?");
                        break;
                    }
                    player.TryUseItem(command.SecondWord);
                    break;


                case "take":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Take what?");
                        break;
                    }
                    player.TryTakeItem(command.SecondWord);
                    break;


                case "drop":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Drop what?");
                        break;
                    }
                    player.TryDropItem(command.SecondWord);
                    break;


                case "inventory":
                    player.PrintInventory();
                    break;


                case "back":
                    player.BackToRoom();
                    break;


                case "move":
                    player.MoveToRoom(command.SecondWord);
                    break;


                case "travel":
                    player.MoveToLocation(command.SecondWord, map);
                    break;


                case "talk" or "talkto":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Talk to who?");
                        break;
                    }
                    player.TryTalkToNpc(command.SecondWord);
                    break;


                case "quit":
                    return false;


                case "help":
                    player.PrintHelp();
                    break;


                default:
                    Console.WriteLine("I don't know that command.");
                    break;
            }
            return true;
        }
    }
}