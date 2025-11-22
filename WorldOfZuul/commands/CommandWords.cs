namespace WorldOfZuul.Commands
{
    public class CommandWords
    {
        public List<string> ValidCommands { get; } = new List<string> {
            "quit", "help", "look", // General commands
            "back", "move", "travel", // Movement commands
            "take", "inspect", "drop", "use", "inventory", // Item commands
            "talk", "talkto" // NPC commands
        };

        public bool IsValidCommand(string command)
        {
            return ValidCommands.Contains(command);
        }
    }

}
