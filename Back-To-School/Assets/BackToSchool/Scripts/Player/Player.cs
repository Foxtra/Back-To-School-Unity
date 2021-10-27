using System;
using System.Collections;
using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Items;
using Assets.BackToSchool.Scripts.Progression;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Weapon;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class Player : MonoBehaviour, IMovable, IShootable, IDamageable
    {
        public event Action<int> AmmoChanged;
        public event Action<float> HealthChanged;
        public event Action Died;

        public PlayerStats PlayerStats;
        public LevelSystem LevelSystem;

        [SerializeField] private float _damageTime = 0.1f;
        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private Animator _animator;
        private Inventory _inventory;
        private Rigidbody _rigidBody;
        private SkinnedMeshRenderer[] _renderers;
        private WeaponController _weaponController;

        private int _currentAmmo;
        private float _currentHealth;
        private bool _isDead;
        private bool _isReloading;

        private void Awake()
        {
            _rigidBody        = GetComponent<Rigidbody>();
            _animator         = GetComponent<Animator>();
            _renderers        = GetComponentsInChildren<SkinnedMeshRenderer>();
            _weaponController = GetComponent<WeaponController>();
            _inventory        = GetComponent<Inventory>();
            _weaponController.SetInventory(_inventory);
        }

        #region Interaction

        public void InitializeAmmoAndHealt()
        {
            _currentAmmo = PlayerStats.MaxAmmo.GetValue();
            AmmoChanged?.Invoke(_currentAmmo);

            _currentHealth = PlayerStats.MaxHealth.GetValue();
            HealthChanged?.Invoke(_currentHealth);
        }

        public float GetHealthValue() => _currentHealth;
        public int   GetAmmoValue()   => _currentAmmo;

        public void SetHealthValue(float health)
        {
            _currentHealth = health;
            HealthChanged?.Invoke(_currentHealth);
        }

        public void SetAmmoValue(int ammo)
        {
            _currentAmmo = ammo;
            AmmoChanged?.Invoke(_currentAmmo);
        }

        public void TakeDamage(float damage)
        {
            damage         -= PlayerStats.Armor.GetValue();
            damage         =  Mathf.Clamp(damage, 0, int.MaxValue);
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth);

            if (_currentHealth == 0 && !_isDead)
            {
                _animator.SetTrigger(AnimationStates.Die);
                _isDead = true;
                Died?.Invoke();
            }
            else if (_currentHealth > 0)
                StartCoroutine(nameof(ShowDamageEffect));
        }

        private IEnumerator ShowDamageEffect()
        {
            yield return new WaitForSeconds(_delayBeforeDamage);
            ChangeColor(Color.red);
            yield return new WaitForSeconds(_damageTime);
            ChangeColor(Color.white);
        }

        private void ChangeColor(Color color)
        {
            foreach (var renderer in _renderers) { renderer.material.color = color; }
        }

        #endregion

        #region Shooting

        public void NextWeapon(bool isNext) { _weaponController.NextWeapon(isNext); }

        public void Fire()
        {
            if (!_isReloading && _currentAmmo != 0)
            {
                _currentAmmo--;
                AmmoChanged?.Invoke(_currentAmmo);
                _weaponController.Shoot(PlayerStats.Damage.GetValue());
            }
        }

        public void Reload()
        {
            _isReloading = true;
            _currentAmmo = PlayerStats.MaxAmmo.GetValue();
            _animator.SetTrigger(AnimationStates.Reload);
            Invoke(nameof(ReloadComplete), _weaponController.ReloadTime);
        }

        private void ReloadComplete()
        {
            _isReloading = false;
            AmmoChanged?.Invoke(_currentAmmo);
        }

        #endregion

        #region Movement

        public void Move(Vector3 direction)
        {
            _animator.SetBool(AnimationStates.IsMoving, true);
            _rigidBody.MovePosition(transform.position + direction * PlayerStats.MoveSpeed.GetValue() * Time.fixedDeltaTime);
        }

        public void Stop() => _animator.SetBool(AnimationStates.IsMoving, false);

        public void Rotate(Vector3 pointToRotate)
        {
            var targetPosition = new Vector3(pointToRotate.x,
                transform.position.y,
                pointToRotate.z);
            transform.LookAt(targetPosition);
        }

        #endregion
    }
}