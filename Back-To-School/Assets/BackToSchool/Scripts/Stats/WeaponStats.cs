using Assets.BackToSchool.Scripts.Parameters;


namespace Assets.BackToSchool.Scripts.Stats
{
    public class WeaponStats
    {
        public Stat FireRate;
        public Stat MaxAmmo;
        public Stat ReloadSpeed;

        public WeaponStats(int fireRate, int maxAmmo, int reloadSpeed)
        {
            FireRate    = new Stat(fireRate);
            MaxAmmo     = new Stat(maxAmmo);
            ReloadSpeed = new Stat(reloadSpeed);
        }

        public WeaponStats() : this(Constants.WeaponStats.InitialFireRate, Constants.WeaponStats.InitialMaxAmmo,
            Constants.WeaponStats.InitialReloadSpeed) { }
    }
}