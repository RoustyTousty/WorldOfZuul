namespace WorldOfZuul.Commands
{
    public class CommandWords
    {
        public List<string> ValidCommands { get; } = new List<string> { "look", "back", "quit", "move", "inspect", "take" };

        public bool IsValidCommand(string command)
        {
            return ValidCommands.Contains(command);
        }
    }

}
