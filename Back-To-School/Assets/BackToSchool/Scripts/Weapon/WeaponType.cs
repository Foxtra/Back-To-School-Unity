using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapon
{
    public class WeaponType : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponEffect;

        public float ReloadTime { get; private set; }
        public float BulletForce { get; private set; }

        public void OnPlayerDeath() => Destroy(_weaponEffect);

        private void Start()
        {
            ReloadTime  = 2f;
            BulletForce = 20f;
        }
    }
}