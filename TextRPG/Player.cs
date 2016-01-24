using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Player:Character
    {
        public Player(float maxHealth, string name):base(maxHealth, name)
        {

        }

        public Player(float maxHealth) : base(maxHealth, "player")
        {

        }

    }
}
