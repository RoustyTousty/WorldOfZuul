using WorldOfZuul.World;
using WorldOfZuul.Entities;
using WorldOfZuul.Commands;

namespace WorldOfZuul
{
    public class Game
    {
        private Player? player;

        public Game()
        {
            /*
            * Build the game world and setup the player.
            */
            Builder builder = new Builder();
            Map map = builder.BuildMapFromJSON();

            /*
            * Initialize player and prompt for a name.
            */
            player = new Player(map.Locations[0]);
            player.PromptPlayerName();
        }

        

        /*   
        * Main play cycle. Loops until end of the game.
        * Manages user input (Commands) and executes them.
        */
        public void Play()
        {
            Parser parser = new();
            player?.PrintWelcome();
            
            bool continuePlaying = true;

            while (continuePlaying)
            {
                Console.WriteLine($"Current room: {player?.CurrentRoom.Name}");
                Console.Write("> ");

                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
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

                    case "quit":
                        continuePlaying = false;
                        break;

                    case "help":
                        player?.PrintHelp();
                        break;

                    default:
                        Console.WriteLine("I don't know that command.");
                        break;
                }
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }
    }
}