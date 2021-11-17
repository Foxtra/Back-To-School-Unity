namespace Assets.BackToSchool.Scripts.Stats
{
    public class WeaponStats
    {
        public static readonly int _initialFireRate = 1;
        public static readonly int _initialMaxAmmo = 1;
        public static readonly int _initialReloadSpeed = 1;

        public Stat FireRate;
        public Stat MaxAmmo;
        public Stat ReloadSpeed;

        public WeaponStats(int fireRate, int maxAmmo, int reloadSpeed)
        {
            FireRate    = new Stat(fireRate);
            MaxAmmo     = new Stat(maxAmmo);
            ReloadSpeed = new Stat(reloadSpeed);
        }

        public WeaponStats() : this(_initialFireRate, _initialMaxAmmo, _initialReloadSpeed) { }
    }
}