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

        public CharacterStats CharacterStats;
        public LevelSystem LevelSystem;

        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private GameObject _shootingPosition;

        //TODO from stats
        [SerializeField] private float _reloadTime = 2.0f;
        [SerializeField] private float _bulletForce = 20.0f;
        [SerializeField] private int _maxAmmo = 10;
        [SerializeField] private float _playerDamage = 1f;
        [SerializeField] private int _maxHealth = 5;
        [SerializeField] private float _damageTime = 0.1f;
        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private Animator _animator;
        private Bullet _bullet;
        private Rigidbody _rigidBody;
        private SkinnedMeshRenderer[] _renderers;
        private WeaponType _weaponType;

        private int _currentAmmo;
        private float _moveSpeed;
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
            CharacterStats = new CharacterStats();
            LevelSystem    = new LevelSystem();

            _currentAmmo = _maxAmmo; //TODO from stats
            AmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
            _moveSpeed     =  5f;
            _currentHealth =  _maxHealth;
            Died           += _weaponType.OnPlayerDeath;
        }

        #region Interaction

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);

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
            foreach (var renderer in _renderers)
            {
                renderer.material.color = color;
            }
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
                _bullet.SetDamage(_playerDamage); //TODO from stats
                _bullet.Launch(_bulletForce);
                _currentAmmo--;
                AmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
            }
        }

        public void Reload()
        {
            _isReloading = true;
            _currentAmmo = _maxAmmo;
            _animator.SetTrigger(AnimationStates.Reload);
            Invoke(nameof(ReloadComplete), _reloadTime);
        }

        private void ReloadComplete()
        {
            _isReloading = false;
            AmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
        }

        #endregion

        #region Movement

        public void Move(Vector3 direction)
        {
            _animator.SetBool(AnimationStates.IsMoving, true);
            _rigidBody.MovePosition(transform.position + direction * _moveSpeed * Time.fixedDeltaTime);
        }

        public void Stop() => _animator.SetBool(AnimationStates.IsMoving, false);

        public void SetMoveSpeed(float moveSpeed) => _moveSpeed = moveSpeed;

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