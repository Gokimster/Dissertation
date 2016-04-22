namespace TextRPG
{
    public class CombatCharacter:Character
    {
        public float maxHealth {get; set;}
        public float currHealth { get; set; }
        public float dmg { get; set;}
        
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

        /// <summary>
        /// Heal the character by a number of points
        /// </summary>
        /// <param name="points"></param>
        public void heal(float points)
        {
            currHealth += modifyHeal(points);
            if (currHealth > maxHealth)
                currHealth = maxHealth;
        }

        /// <summary>
        /// Return the point amount of a heal after modifying it taking into acount stats
        /// </summary>
        /// <param name="points"></param>
        protected float modifyHeal(float points)
        {
            //TO-DO: change heal based on stats, debuffs etc
            return points;
        }

        /// <summary>
        /// Take a number of points of damage
        /// </summary>
        /// <param name="points"></param>
        public void takeDmg(float points)
        {
            currHealth -= modifyDmgTaken(points);
            if (currHealth <= 0)
                doDeath();
        }

        /// <summary>
        /// Return the damage done by the character
        /// </summary>
        /// <param name="points"></param>
        public float getDmgDone()
        {
            return modifyDmgDone(dmg);
        }

        /// <summary>
        /// Return the points of damage taken after modifications based on stats
        /// </summary>
        /// <param name="points"></param>
        protected float modifyDmgTaken(float points)
        {
            //TO-DO: change dmg taken based on stats, debuffs etc
            return points;
        }

        /// <summary>
        /// Return the points of damage done after modifications based on stats
        /// </summary>
        /// <param name="points"></param>
        protected float modifyDmgDone(float points)
        {
            //TO-DO: change dmg taken based on stats, debuffs etc
            return points;
        }

        /// <summary>
        /// Do the death behaviour of a character
        /// </summary>
        protected void doDeath()
        {
            currHealth = 0;
        }

        /// <summary>
        /// Check if a character is dead or not
        /// </summary>
        public bool isDead()
        {
            return currHealth <= 0;
        }
    }
}
