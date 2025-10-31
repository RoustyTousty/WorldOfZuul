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



        /*
        * Moves the player to the room connected by the specified exit name.
        * Returns true if the move was successful, false otherwise.
        */
        public bool Move(string exitName)
        {
            Room? nextRoom = CurrentRoom.GetExitsRoom(exitName);
            if (nextRoom == null)
                return false;

            PreviousRoom = CurrentRoom;
            CurrentRoom = nextRoom;
            return true;
        }



        /*
        * Moves the player to a new location, resetting the current room to the starting room of that location.
        */
        public bool MoveToLocation(Location newLocation)
        {
            if (newLocation == null) return false;

            CurrentLocation = newLocation;
            PreviousRoom = null;
            CurrentRoom = newLocation.StartingRoom;
            return true;
        }



        /*
        * Moves the player back to the previous room.
        */
        public bool GoBack()
        {
            if (PreviousRoom == null)
                return false;

            var temp = CurrentRoom;
            CurrentRoom = PreviousRoom;
            PreviousRoom = temp;
            return true;
        }



        /*
        * Prints the description of the current room and its exits.
        */
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
