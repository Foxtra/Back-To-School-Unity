using Assets.BackToSchool.Scripts.Stats;


namespace Assets.BackToSchool.Scripts.Interfaces
{
    public interface IWeapon
    {
        WeaponStats WeaponStats { get; set; }
        int CurrentAmmo { get; set; }
        void Attack(float damage);
        void ReloadFinished();
    }
}