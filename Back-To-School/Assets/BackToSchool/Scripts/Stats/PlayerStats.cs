using Assets.BackToSchool.Scripts.Parameters;


namespace Assets.BackToSchool.Scripts.Stats
{
    public class PlayerStats : CharacterStats
    {
        public Stat Armor;

        public PlayerStats(int armor, int damage, int maxHealth, int moveSpeed) : base(damage,
            maxHealth, moveSpeed) => Armor = new Stat(armor);

        public PlayerStats() : this(Constants.InitialArmor, Constants.InitialDamage, Constants.InitialMaxHealth,
            Constants.InitialMoveSpeed) { }
    }
}