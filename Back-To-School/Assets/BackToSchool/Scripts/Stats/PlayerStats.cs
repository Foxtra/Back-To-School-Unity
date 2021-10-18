namespace Assets.BackToSchool.Scripts.Stats
{
    public class PlayerStats : CharacterStats
    {
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

        public PlayerStats() : this(0, 1, 1, 10, 5, 5, 1) { }
    }
}