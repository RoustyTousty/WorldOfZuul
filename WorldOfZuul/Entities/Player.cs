using WorldOfZuul.World;
using WorldOfZuul.Item;

namespace WorldOfZuul.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public Location CurrentLocation { get; private set; }
        public Room CurrentRoom { get; private set; }
        public Room? PreviousRoom { get; private set; }
        // public Inventory Inventory { get; } = new();

        public Player(string name, Location startingLocation)
        {
            Name = name;
            CurrentRoom = startingLocation.StartingRoom;
            CurrentLocation = startingLocation;
        }

        public bool Move(string exitName)
        {
            if (!CurrentRoom.Exits.ContainsKey(exitName))
                return false;

            PreviousRoom = CurrentRoom;
            CurrentRoom = CurrentRoom.GetExitsRoom(exitName);
            return true;
        }

        public bool MoveToLocation(Location newLocation)
        {
            if (newLocation == null) return false;
            
            CurrentLocation = newLocation;
            PreviousRoom = null;
            CurrentRoom = newLocation.StartingRoom;
            return true;
        }

        public bool GoBack()
        {
            if (PreviousRoom == null)
                return false;

            var temp = CurrentRoom;
            CurrentRoom = PreviousRoom;
            PreviousRoom = temp;
            return true;
        }

        public void Look()
        {
            Console.WriteLine(CurrentRoom.Description);
            Console.Write("Exits: ");
            foreach (var exit in CurrentRoom.Exits)
                Console.Write(exit.Key + ", ");
            Console.WriteLine();
        }
    }
}
