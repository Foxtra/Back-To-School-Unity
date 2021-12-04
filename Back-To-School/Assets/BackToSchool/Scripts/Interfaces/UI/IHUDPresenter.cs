using Assets.BackToSchool.Scripts.Parameters;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IHUDPresenter : IView
    {
        public void OnHealthChanged(float newCurrentHealth);
        public void OnMaxHealthChanged(int newMaxHealth);
        public void OnWeaponChanged(int weaponNumber);
        public void OnAmmoChanged(int newAmmoValue);
        public void OnMaxAmmoChanged(int newMaxAmmoValue);
        public void InitializeObjectives(ObjectiveParameters initializeParams);
        public void OnTimeChanged(float time);
        public void OnEnemiesKillChanged(int warriorsToKill, int shamansToKill);
        public void OnLevelChanged(int newLevel);
        public void OnExpChanged(float newSliderValue);
        public void OnArmorChanged(int newValue);
        public void OnDamageChanged(int newValue);
        public void OnMoveSpeedChanged(int newValue);
    }
}