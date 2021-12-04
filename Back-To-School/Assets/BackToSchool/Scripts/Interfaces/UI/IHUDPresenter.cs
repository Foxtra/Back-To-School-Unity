namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IHUDPresenter : IView
    {
        void OnHealthChanged(float newCurrentHealth);
        void OnMaxHealthChanged(int newMaxHealth);
        void OnWeaponChanged(int weaponNumber);
        void OnAmmoChanged(int newAmmoValue);
        void OnMaxAmmoChanged(int newMaxAmmoValue);
        void OnLevelChanged(int newLevel);
        void OnExpChanged(float newSliderValue);
        void OnArmorChanged(int newValue);
        void OnDamageChanged(int newValue);
        void OnMoveSpeedChanged(int newValue);
    }
}