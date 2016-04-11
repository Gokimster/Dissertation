namespace TextRPG
{
    public class Item
    {
        public string description { get; set; }
        public string name { get; set; }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        //constructors
        public Item()
        {

        }
        public Item(string name)
        {
            this.name = name;
        }
        public Item(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
        
    }
}
