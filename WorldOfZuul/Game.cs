using System.Linq;
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

            // If the JSON already created a room with id "ENI", replace it and update all exits pointing to it.
            var existingLocationWithEni = map.Locations.Values.FirstOrDefault(loc => loc.Rooms.ContainsKey("ENI"));
            if (existingLocationWithEni != null)
            {
                Room oldEniRoom = existingLocationWithEni.GetRoom("ENI")!;
                existingLocationWithEni.SetRoom(eniOffice);
                
                // Update all exits in the map that were pointing to the old ENI room to point to the new one.
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
            }
            else
            {
                Location milanLocation = map.GetLocation("milan") ?? new Location("milan", "Milan", "A bustling city in Italy.", "ENI");
                milanLocation.SetRoom(eniOffice);
                map.SetLocation(milanLocation);
            }

            /*
            * Initialize player and prompt for a name.
            */
            player = new Player(map.GetLocation(map.StartingLocationId)!);
            player.PrintEmptySpace(50);
            player.PromptPlayerName();
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
                    player?.PrintRoom();
                    break;


                case "inspect":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Inspect what?");
                    }
                    else
                    {
                        string response = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, "inspect") ?? "You can't inspect that.";
                        Console.WriteLine(response);
                    }
                    break;


                case "take":
                    // if (command.SecondWord == null)
                    // {
                    //     Console.WriteLine("Take what?");
                    //     break;
                    // }
                    // player?.TryTakeItem(command.SecondWord);
                    break;


                case "open":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Open what?");
                    }
                    else
                    {
                        string response = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, $"open {command.SecondWord}") ?? "You can't open that.";
                        Console.WriteLine(response);
                    }
                    break;


                case "read":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Read what?");
                    }
                    else
                    {
                        string response = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, $"read {command.SecondWord}") ?? "You can't read that.";
                        Console.WriteLine(response);
                    }
                    break;


                case "answer":
                    if (command.SecondWord == null)
                    {
                        string response = player?.CurrentRoom.HandleInteractiveAction("phone", "answer") ?? "There's nothing to answer.";
                        Console.WriteLine(response);
                    }
                    else
                    {
                        string response = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, "answer") ?? "You can't answer that.";
                        Console.WriteLine(response);
                    }
                    break;


                case "push":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Push what?");
                    }
                    else
                    {
                        string response = player?.CurrentRoom.HandleInteractiveAction(command.SecondWord, $"push {command.SecondWord}") ?? "You can't push that.";
                        Console.WriteLine(response);
                    }
                    break;


                case "inventory":
                    player?.PrintInventory();
                    break;


                case "back":
                    player?.BackToRoom();
                    break;


                case "move":
                    player?.MoveToRoom(command.SecondWord);
                    break;


                case "travel":
                    player?.MoveToLocation(command.SecondWord, map);
                    break;


                case "talk" or "talkto":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Talk to who?");
                        break;
                    }
                    player?.TryTalkToNpc(command.SecondWord);
                    break;


                case "quit":
                    return false;


                case "help":
                    player?.PrintHelp();
                    break;


                default:
                    Console.WriteLine("I don't know that command.");
                    break;
            }
            return true;
        }
    }
}