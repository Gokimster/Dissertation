using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Enemy:Character
    {
        public Enemy(float maxHealth, string name): base(maxHealth, name)
        {

        }

        public Enemy(float maxHealth, float currHealth, string name) : base(maxHealth, currHealth, name)
        {

        }
    }
}
