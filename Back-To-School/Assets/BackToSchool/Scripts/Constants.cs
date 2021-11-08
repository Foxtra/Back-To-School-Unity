namespace Assets.BackToSchool.Scripts
{
    public static class Constants
    {
        public enum AnimationStates
        {
            IsMoving,
            Reload,
            Die,
            GetDamage,
            Attack
        }

        public enum SaveParams
        {
            PlayerLevel,
            PlayerExperience,
            PlayerHealth,
            PlayerAmmo,
            PlayerWeapon,
            IsSaveDataExists
        }

        public enum SceneNames
        {
            MainMenu,
            MainScene
        }
    }
}