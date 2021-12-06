using Assets.BackToSchool.Scripts.Stats;


namespace Assets.BackToSchool.Scripts.Interfaces.Components
{
    public interface IWeapon
    {
        public WeaponStats WeaponStats { get; set; }
        public int CurrentAmmo { get; set; }
        public void Attack(float damage);
        public void ReloadFinished();
    }
}