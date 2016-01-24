using System.Xml.Linq;

namespace TextRPG
{
    public class Character
    {
        protected float maxHealth;
        protected float currHealth;
        public string name { get; set; }

        public Character(float maxHealth, string name)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            currHealth = maxHealth;
        }

        public Character(float maxHealth, float currHealth, string name)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            this.currHealth = currHealth;
        }

        public void loadCharacter(XElement elem)
        {
            
        }

        public void heal(float points)
        {
            currHealth += modifyHeal(points);
            if (currHealth > maxHealth)
                currHealth = maxHealth;
        }

        protected float modifyHeal(float points)
        {
            //TO-DO: change heal based on stats, debuffs etc
            return points;
        }

        public void takeDmg(float points)
        {
            currHealth -= modifyDmg(points);
            if (currHealth <= 0)
                doDeath();
        }

        protected float modifyDmg(float points)
        {
            //TO-DO: change dmg taken based on stats, debuffs etc
            return points;
        }

        protected void doDeath()
        {
        }

        public bool isDead()
        {
            return currHealth > 0;
        }
    }
}
