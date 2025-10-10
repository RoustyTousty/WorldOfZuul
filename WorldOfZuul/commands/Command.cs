using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class Command
    {
        public string Name { get; }
        public string? SecondWord { get; } // this might be used for future expansions, such as "take apple".

        public Command(string name, string? secondWord = null)
        {
            Name = name;
<<<<<<< Updated upstream:WorldOfZuul/commands/Command.cs
            SecondWord = secondWord;
=======
            SecondWord = secondWord;//daje roma
>>>>>>> Stashed changes:WorldOfZuul/Command.cs
            SecondWord = secondWord;   //change test comment hi :333 
>>>>>>> aaf47066658e9a2006416a8b2344f2f306ffa202
        }
    }
}
