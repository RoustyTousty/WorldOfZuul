namespace WorldOfZuul.Entities
{
    /*
    * Represents a dialogue tree for NPC conversations.
    */
    public class DialogueNode
    {
        public string Id { get; }
        public string Text { get; }
        public List<DialogueResponse> Responses { get; }

        public DialogueNode(string id, string text, List<DialogueResponse>? responses = null)
        {
            Id = id;
            Text = text;
            Responses = responses ?? new List<DialogueResponse>();
        }
    }
}