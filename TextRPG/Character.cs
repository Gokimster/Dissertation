using System.Xml.Linq;

namespace TextRPG
{
    public class Character
    {
        public string name { get; set; }

        public Character()
        {

        }

        public Character(string name)
        {
            this.name = name;
        }
    }
}
