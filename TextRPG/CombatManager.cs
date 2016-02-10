namespace TextRPG
{
    public static class CombatManager
    {
        public static void doCombat(Enemy e)
        {
            fightTillDeath(e);
        }

        public static void fightTillDeath(Enemy e)
        {
            while (!e.isDead() && !Player.Instance.isDead())
            {
                attackPhaseSimultaneous(e);
            }
        }

        private static void playerAttack(Enemy e)
        {
            e.takeDmg(Player.Instance.getDmgDone());
        }

        private static void enemyAttack(Enemy e)
        {
            Player.Instance.takeDmg(e.getDmgDone());
        }

        public static void attackPhaseSimultaneous(Enemy e)
        {
            playerAttack(e);
            enemyAttack(e);
        }
    }
}
