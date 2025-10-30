using WorldOfZuul.World;
using WorldOfZuul.Entities;

namespace WorldOfZuul
{
    public class Game
    {
        private const string DEFAULT_PLAYER_NAME = "Bomboclat";

        private Player? player;

        public Game()
        {
            Builder builder = new Builder();
            Map map = builder.BuildMapFromJSON();

            string name = PromptPlayerName();

            player = new Player(name, map.Locations[0]);
        }

        public void Play()
        {
            Parser parser = new();
            PrintWelcome();

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

                switch (command.Name)
                {
                    case "look":
                        player?.Look();
                        break;

                    case "back":
                        if (player != null && player.GoBack())
                            Console.WriteLine($"You return to {player.CurrentRoom.Name}.");
                        else
                            Console.WriteLine("You can't go back from here!");
                        break;

                    case "move":
                        if (command.SecondWord == null)
                        {
                            Console.WriteLine("Move where?");
                        }
                        else if (player != null && player.Move(command.SecondWord))
                        {
                            Console.WriteLine($"You move to {player.CurrentRoom.Name}.");
                        }
                        else
                        {
                            Console.WriteLine($"You can't go to '{command.SecondWord}'.");
                        }
                        break;

                    case "quit":
                        continuePlaying = false;
                        break;

                    case "help":
                        PrintHelp();
                        break;

                    default:
                        Console.WriteLine("I don't know that command.");
                        break;
                }
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }

        private static string PromptPlayerName()
        {
            Console.WriteLine("Enter your name!");
            Console.Write("> ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                name = DEFAULT_PLAYER_NAME;
                Console.WriteLine($"No name entered. You shall be known as {name}!");
            }
            else
            {
                Console.WriteLine($"Welcome, {name}!");
            }

            return name;
        }

        private static void PrintWelcome()
        {
            Console.WriteLine("Welcome to the World of Zuul!");
            Console.WriteLine("World of Zuul is a new, incredibly boring adventure game.");
            PrintHelp();
            Console.WriteLine();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine(" - move [exitName]");
            Console.WriteLine(" - back");
            Console.WriteLine(" - look");
            Console.WriteLine(" - help");
            Console.WriteLine(" - quit");
        }
    }
}