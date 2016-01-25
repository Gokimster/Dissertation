using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class CombatCharacter:Character
    {
        protected float maxHealth;
        public float currHealth { get; set; }
        protected float dmg;

        public CombatCharacter()
        {

        }
        public CombatCharacter(float maxHealth, string name, float dmg):base(name)
        {
            this.dmg = dmg;
            this.maxHealth = maxHealth;
            currHealth = maxHealth;
        }

        public CombatCharacter(float maxHealth, float currHealth, string name, float dmg):base(name)
        {
            this.dmg = dmg;
            this.maxHealth = maxHealth;
            this.currHealth = currHealth;
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
            currHealth -= modifyDmgTaken(points);
            if (currHealth <= 0)
                doDeath();
        }

        public float getDmgDone()
        {
            return modifyDmgDone(dmg);
        }

        protected float modifyDmgTaken(float points)
        {
            //TO-DO: change dmg taken based on stats, debuffs etc
            return points;
        }

        protected float modifyDmgDone(float points)
        {
            //TO-DO: change dmg taken based on stats, debuffs etc
            return points;
        }

        protected void doDeath()
        {
        }

        public bool isDead()
        {
            return currHealth <= 0;
        }
    }
}
