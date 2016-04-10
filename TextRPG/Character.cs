using System.Xml.Linq;

namespace TextRPG
{
    public class Character
    {
        public string name { get; set; }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        public Character()
        {

        }

        public Character(string name)
        {
            this.name = name;
        }
    }
}
