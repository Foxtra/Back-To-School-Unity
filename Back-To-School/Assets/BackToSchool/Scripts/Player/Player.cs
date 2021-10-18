using System;
using System.Collections;
using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Weapon;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class Player : MonoBehaviour, IMovable, IShootable, IDamageable
    {
        public event Action<int, int> AmmoChanged;
        public event Action<float, int> HealthChanged;
        public event Action Died;

        public PlayerStats PlayerStats;
        public LevelSystem LevelSystem;

        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private GameObject _shootingPosition;
        [SerializeField] private float _damageTime = 0.1f;
        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private Animator _animator;
        private Bullet _bullet;
        private Rigidbody _rigidBody;
        private SkinnedMeshRenderer[] _renderers;
        private WeaponType _weaponType;

        private int _currentAmmo;
        private float _currentHealth;
        private bool _isDead;
        private bool _isReloading;

        private void Awake()
        {
            _rigidBody  = GetComponent<Rigidbody>();
            _animator   = GetComponent<Animator>();
            _renderers  = GetComponentsInChildren<SkinnedMeshRenderer>();
            _weaponType = GetComponent<WeaponType>();
        }

        private void Start()
        {
            _currentAmmo = PlayerStats.MaxAmmo.GetValue();
            AmmoChanged?.Invoke(_currentAmmo, PlayerStats.MaxAmmo.GetValue());

            _currentHealth = PlayerStats.MaxHealth.GetValue();
            HealthChanged?.Invoke(_currentHealth, PlayerStats.MaxHealth.GetValue());

            Died += _weaponType.OnPlayerDeath;
        }

        #region Interaction

        public void TakeDamage(float damage)
        {
            damage         -= PlayerStats.Armor.GetValue();
            damage         =  Mathf.Clamp(damage, 0, int.MaxValue);
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth, PlayerStats.MaxHealth.GetValue());

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

        public void Fire()
        {
            if (!_isReloading && _currentAmmo != 0)
            {
                _bullet                    = Instantiate(_bulletPrefab);
                _bullet.transform.position = _shootingPosition.transform.position;
                _bullet.transform.rotation = _shootingPosition.transform.rotation;
                _bullet.SetDamage(PlayerStats.Damage.GetValue());
                _bullet.Launch(_weaponType.BulletForce);
                _currentAmmo--;
                AmmoChanged?.Invoke(_currentAmmo, PlayerStats.MaxAmmo.GetValue());
            }
        }

        public void Reload()
        {
            _isReloading = true;
            _currentAmmo = PlayerStats.MaxAmmo.GetValue();
            _animator.SetTrigger(AnimationStates.Reload);
            Invoke(nameof(ReloadComplete), _weaponType.ReloadTime);
        }

        private void ReloadComplete()
        {
            _isReloading = false;
            AmmoChanged?.Invoke(_currentAmmo, PlayerStats.MaxAmmo.GetValue());
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