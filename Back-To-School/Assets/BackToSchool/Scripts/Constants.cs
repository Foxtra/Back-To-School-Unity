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

        public enum EnemyStates
        {
            Patrolling,
            Chasing,
            Attacking
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

        public static float MinXpos = -49f;
        public static float MaxXpos = 49f;
        public static float MinZpos = -49f;
        public static float MaxZpos = 49f;
    }
}