namespace Assets.BackToSchool.Scripts.Stats
{
    public class PlayerStats : CharacterStats
    {
        public static readonly int _initialArmor = 0;
        public static readonly int _initialDamage = 1;
        public static readonly int _initialMaxHealth = 5;
        public static readonly int _initialMoveSpeed = 4;

        public Stat Armor;

        public PlayerStats(int armor, int damage, int maxHealth, int moveSpeed) : base(damage,
            maxHealth, moveSpeed) => Armor = new Stat(armor);

        public PlayerStats() : this(_initialArmor, _initialDamage, _initialMaxHealth, _initialMoveSpeed) { }
    }
}