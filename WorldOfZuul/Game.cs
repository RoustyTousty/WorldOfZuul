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
                    player.PrintRoom();
                    break;


                case "inspect":
                    if (command.SecondWord == null)
                    {
                        Console.WriteLine("Inspect what?");
                        break;
                    }
                    player.TryInspectItem(command.SecondWord);
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