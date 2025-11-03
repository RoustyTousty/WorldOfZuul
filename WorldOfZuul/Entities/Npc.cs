namespace WorldOfZuul.Entities
{
    public class Npc
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }

        private Dictionary<string, DialogueNode> DialogueNodes;
        private DialogueNode? CurrentNode;

        public Npc(string id, string name, string description, List<DialogueNode>? dialogueNodes = null)
        {
            Id = id;
            Name = name;
            Description = description;
            DialogueNodes = dialogueNodes?.ToDictionary(n => n.Id, n => n) ?? new Dictionary<string, DialogueNode>();
        }



        /*
        * Starts a dialogue with the NPC.
        */
        public void StartDialogue()
        {
            if (DialogueNodes.Count == 0)
            {
                Console.WriteLine($"{Name} has nothing to say right now.");
                return;
            }

            CurrentNode = DialogueNodes.Values.First();
            DisplayCurrentNode();
        }



        /*
        * Displays the current dialogue node and handles player responses.
        */
        private void DisplayCurrentNode()
        {
            if (CurrentNode == null) return;

            Console.WriteLine($"\n{nameof(Name)}: {CurrentNode.Text}");

            if (CurrentNode.Responses == null || CurrentNode.Responses.Count == 0)
                return;

            for (int i = 0; i < CurrentNode.Responses.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {CurrentNode.Responses[i].Text}");
            }

            Console.Write("> ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int choice) &&
                choice > 0 && choice <= CurrentNode.Responses.Count)
            {
                string nextId = CurrentNode.Responses[choice - 1].Next;
                if (nextId == "end")
                {
                    Console.WriteLine($"{Name}: Take care!");
                    CurrentNode = null;
                    return;
                }

                if (DialogueNodes.TryGetValue(nextId, out var nextNode))
                {
                    CurrentNode = nextNode;
                    DisplayCurrentNode();
                }
                else
                {
                    Console.WriteLine("They don't seem to respond further.");
                }
            }
        }
    }
}
