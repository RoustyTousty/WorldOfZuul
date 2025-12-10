using WorldOfZuul;

Console.Title = "World of Zuul - Interactive Adventure";

#pragma warning disable CA1416 // Suppress Windows-only platform warnings
if (OperatingSystem.IsWindows())
{
    Console.WindowWidth = 120;
    Console.WindowHeight = 40;
}
#pragma warning restore CA1416

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
Console.WriteLine("                                           WELCOME TO CLEAN HANDS");
Console.WriteLine("                               A GAME BASED ON ONE OF THE BIGGEST ITALIAN MAFIA TRIALS");
Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════");
Console.ResetColor();
Console.WriteLine();

Game game = new Game();
game.Play();
