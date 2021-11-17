using System;


namespace Assets.BackToSchool.Scripts.Player
{
    [Serializable]
    public class PlayerData
    {
        public PlayerData() { }

        public PlayerData(int playerLevel, int playerExperience, int playerAmmo, int playerWeapon, float playerHealth)
        {
            PlayerLevel      = playerLevel;
            PlayerExperience = playerExperience;
            PlayerAmmo       = playerAmmo;
            PlayerWeapon     = playerWeapon;
            PlayerHealth     = playerHealth;
        }

        public int PlayerLevel { get; set; }
        public int PlayerExperience { get; set; }
        public int PlayerAmmo { get; set; }
        public int PlayerWeapon { get; set; }
        public float PlayerHealth { get; set; }
    }
}