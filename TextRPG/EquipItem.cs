namespace TextRPG
{
    class EquipItem:Item
    {
        public float dmgBonus { get; set; }

        public EquipItem():base()
        {

        }
        
        public EquipItem(string name, string description, float dmgBonus):base(name, description)
        {
            this.dmgBonus = dmgBonus;
        }
    }
}
