namespace WorldOfZuul.Entities
{
    public class CaseState
    {
        private HashSet<string> clues = new();

        public void AddClue(string clueId)
        {
            clues.Add(clueId);
        }

        public bool HasClue(string clueId)
        {
            return clues.Contains(clueId);
        }

        // Winning condition
        public bool CanExposeTruth =>
            HasClue("ledger") &&
            HasClue("bribe_note") &&
            HasClue("witness_testimony");

        public bool IsSolved { get; private set; }

        public void Solve()
        {
            IsSolved = true;
        }
    }
}
