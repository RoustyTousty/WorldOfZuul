using System;

namespace WorldOfZuul.World
{
    /*
     * Represents an interactive object inside a room. Objects accept verbs
     * (strings) and return a response plus an optional side-effect action
     * which mutates the room's RoomState.
     */
    public class InteractiveObject
    {
        public string Id { get; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        private readonly Func<RoomState, string, (string, Action<RoomState>?)> handler;

        public InteractiveObject(string id, string name, string shortDescription, string longDescription,
                                 Func<RoomState, string, (string, Action<RoomState>?)> handler)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public (string response, Action<RoomState>? sideEffect) Execute(RoomState state, string verb)
        {
            return handler(state, verb);
        }
    }
}
