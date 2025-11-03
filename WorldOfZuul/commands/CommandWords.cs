namespace WorldOfZuul.Commands
{
    public class CommandWords
    {
        public List<string> ValidCommands { get; } = new List<string> {
            "quit", "help", "look", // General commands
            "back", "move", // Movement commands
            "take", "inspect", "inventory", // Item commands
            "talk", "talkto" // NPC commands
        };

        public bool IsValidCommand(string command)
        {
            return ValidCommands.Contains(command);
        }
    }

}
