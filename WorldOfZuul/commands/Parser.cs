namespace WorldOfZuul.Commands
{
    public class Parser
    {
        private readonly CommandWords commandWords = new();

        public Command? GetCommand(string inputLine)
        {
            if (string.IsNullOrWhiteSpace(inputLine))
                return null;

            string[] words = inputLine.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0 || !commandWords.IsValidCommand(words[0]))
                return null;

            string? secondWord = words.Length > 1 ? words[1] : null;
            return new Command(words[0], secondWord);
        }
    }
}
