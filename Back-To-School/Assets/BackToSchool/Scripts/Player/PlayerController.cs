using System;
using System.Collections;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Items;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Weapon;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IMovable, IShootable, IDamageable
    {
        public event Action<float> HealthChanged;
        public event Action Died;

        public Inventory Inventory;
        public WeaponController WeaponController;

        [SerializeField] private float _damageTime = 0.1f;
        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private Animator _animator;
        private PlayerStats _playerStats;
        private Rigidbody _rigidBody;
        private SkinnedMeshRenderer[] _renderers;

        private float _currentHealth;
        private bool _isDead;

        public void Initialize(IPlayerInput playerInput, PlayerStats playerStats)
        {
            _playerStats = playerStats;

            playerInput.Reloaded            += WeaponController.Reload;
            WeaponController.WeaponReloaded += Reload;
            playerInput.Fired               += Fire;
            playerInput.Stopped             += Stop;
            playerInput.Moved               += Move;
            playerInput.Rotated             += Rotate;
            playerInput.WeaponChanged       += NextWeapon;
        }

        private void Awake()
        {
            _rigidBody       = GetComponent<Rigidbody>();
            _animator        = GetComponent<Animator>();
            _renderers       = GetComponentsInChildren<SkinnedMeshRenderer>();
            WeaponController = GetComponent<WeaponController>();
            Inventory        = GetComponent<Inventory>();
            WeaponController.SetInventory(Inventory);
        }

        #region Shooting

        public void Reload()
        {
            if (!_isDead)
                _animator.SetTrigger(Constants.AnimationStates.Reload.ToString());
        }

        public void ReloadFinished() => WeaponController.ReloadComplete();

        public void Fire()
        {
            if (!_isDead)
                WeaponController.Shoot(_playerStats.Damage.GetValue());
        }

        public void NextWeapon(bool isNext)
        {
            if (!_isDead)
                WeaponController.NextWeapon(isNext);
        }

        #endregion

        #region Interaction

        public void InitializeHealth()
        {
            _currentHealth = _playerStats.MaxHealth.GetValue();
            HealthChanged?.Invoke(_currentHealth);
        }

        public float GetHealthValue() => _currentHealth;

        public void SetHealthValue(float health)
        {
            _currentHealth = health;
            HealthChanged?.Invoke(_currentHealth);
        }

        public void TakeDamage(float damage)
        {
            damage         -= _playerStats.Armor.GetValue();
            damage         =  Mathf.Clamp(damage, 0, int.MaxValue);
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0 && !_isDead)
            {
                _animator.SetTrigger(Constants.AnimationStates.Die.ToString());
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

        #region Movement

        public void Move(Vector3 direction)
        {
            if (!_isDead)
            {
                _animator.SetBool(Constants.AnimationStates.IsMoving.ToString(), true);
                _rigidBody.MovePosition(transform.position + direction * _playerStats.MoveSpeed.GetValue() * Time.fixedDeltaTime);
            }
        }

        public void Stop() => _animator.SetBool(Constants.AnimationStates.IsMoving.ToString(), false);

        public void Rotate(Vector3 pointToRotate)
        {
            if (!_isDead)
            {
                var targetPosition = new Vector3(pointToRotate.x,
                    transform.position.y,
                    pointToRotate.z);
                transform.LookAt(targetPosition);
            }
        }

        #endregion
    }
}