namespace WorldOfZuul.Entities
{
    /*
    * Represents a response option in a dialogue node.
    */
    public class DialogueResponse
    {
        public string Text { get; }
        public string Next { get; }

        public DialogueResponse(string text, string next)
        {
            Text = text;
            Next = next;
        }
    }
}