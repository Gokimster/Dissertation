namespace TextRPG
{
    public class Enemy:CombatCharacter
    {
        public Enemy(float maxHealth, string name, float dmg): base(maxHealth, name, dmg)
        {
        }

        public Enemy(float maxHealth, float currHealth, string name, float dmg) : base(maxHealth, currHealth, name, dmg)
        {

        }
    }
}
