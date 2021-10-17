using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapon
{
    public class WeaponType : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponEffect;

        public void OnPlayerDeath() => Destroy(_weaponEffect);
    }
}