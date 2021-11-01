namespace Assets.BackToSchool.Scripts.Stats
{
    public class PlayerStats : CharacterStats
    {
        public static readonly int _initialArmor = 0;
        public static readonly int _initialDamage = 1;
        public static readonly int _initialFireRate = 1;
        public static readonly int _initialMaxAmmo = 5;
        public static readonly int _initialMaxHealth = 5;
        public static readonly int _initialMoveSpeed = 4;
        public static readonly int _initialReloadSpeed = 1;

        public Stat Armor;
        public Stat FireRate;
        public Stat MaxAmmo;
        public Stat ReloadSpeed;

        public PlayerStats(int armor, int damage, int fireRate, int maxAmmo, int maxHealth, int moveSpeed, int reloadSpeed) : base(damage,
            maxHealth, moveSpeed)
        {
            Armor       = new Stat(armor);
            FireRate    = new Stat(fireRate);
            MaxAmmo     = new Stat(maxAmmo);
            ReloadSpeed = new Stat(reloadSpeed);
        }

        public PlayerStats() : this(_initialArmor, _initialDamage, _initialFireRate, _initialMaxAmmo, _initialMaxHealth, _initialMoveSpeed,
            _initialReloadSpeed) { }
    }
}