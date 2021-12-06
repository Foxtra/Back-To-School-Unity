using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Interfaces.Game;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Items;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Weapons;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public event Action<float> HealthChanged;
        public event Action Died;
        public event Action<int> AmmoChanged;
        public event Action<int> WeaponChanged;
        public event Action<int> MaxAmmoChanged;

        private Animator _animator;
        private IPlayerInput _playerInput;
        private IAudioManager _audioManager;
        private Inventory _inventory;
        private PlayerStats _playerStats;
        private Rigidbody _rigidBody;
        private SkinnedMeshRenderer[] _renderers;
        private IWeaponController _weaponController;

        private float _currentHealth;
        private bool _isDead;

        public void Initialize(IPlayerInput playerInput, PlayerStats playerStats, PlayerData playerData, IAudioManager audioManager)
        {
            _playerStats  = playerStats;
            _playerInput  = playerInput;
            _audioManager = audioManager;

            _playerInput.Reloaded            += _weaponController.Reload;
            _weaponController.WeaponReloaded += Reload;
            _playerInput.Fired               += Fire;
            _playerInput.Stopped             += Stop;
            _playerInput.Moved               += Move;
            _playerInput.Rotated             += Rotate;
            _playerInput.WeaponChanged       += NextWeapon;

            _weaponController.AmmoChanged    += OnAmmoChanged;
            _weaponController.MaxAmmoChanged += OnMaxAmmoChanged;
            _weaponController.WeaponChanged  += OnWeaponChanged;

            if (playerData.PlayerHealth != 0)
                _currentHealth = playerData.PlayerHealth;
            else
                _currentHealth = _playerStats.MaxHealth.GetValue();

            _weaponController.InitializeWeapon(playerData.PlayerWeapon);
            _weaponController.InitializeAmmo(playerData.PlayerAmmo);
            _weaponController.InitializeAudioManager(_audioManager);
        }

        public void UpdateHUD()
        {
            HealthChanged?.Invoke(_currentHealth);
            _weaponController.UpdateHUD();
        }

        private void Awake()
        {
            _rigidBody        = GetComponent<Rigidbody>();
            _animator         = GetComponent<Animator>();
            _renderers        = GetComponentsInChildren<SkinnedMeshRenderer>();
            _weaponController = GetComponent<WeaponController>();
            _inventory        = GetComponent<Inventory>();
            _weaponController.SetInventory(_inventory);
        }

        private void OnDestroy()
        {
            _playerInput.Reloaded            -= _weaponController.Reload;
            _weaponController.WeaponReloaded -= Reload;
            _playerInput.Fired               -= Fire;
            _playerInput.Stopped             -= Stop;
            _playerInput.Moved               -= Move;
            _playerInput.Rotated             -= Rotate;
            _playerInput.WeaponChanged       -= NextWeapon;

            _weaponController.AmmoChanged    -= OnAmmoChanged;
            _weaponController.MaxAmmoChanged -= OnMaxAmmoChanged;
            _weaponController.WeaponChanged  -= OnWeaponChanged;
        }

        #region Shooting

        public void Reload()
        {
            if (_isDead)
                return;

            _animator.SetTrigger(EAnimTriggers.Reload.ToStringCached());
            var animTime = Array.Find(_animator.runtimeAnimatorController.animationClips,
                clip => clip.name == EPlayerAnimNames.Reload.ToStringCached()).length;
            WaitWhileReloading(Mathf.RoundToInt(animTime * 1000f));
        }

        public void ReloadFinished() => _weaponController.ReloadComplete();

        public void Fire()
        {
            if (_isDead)
                return;

            _weaponController.Shoot(_playerStats.Damage.GetValue());
        }

        public void NextWeapon(bool isNext)
        {
            if (!_isDead)
                _weaponController.NextWeapon(isNext);
        }

        #endregion

        #region Interaction

        public float GetHealthValue()       => _currentHealth;
        public int   GetAmmoValue()         => _weaponController.GetAmmoValue();
        public int   GetActiveWeaponIndex() => _weaponController.GetWeaponIndex();

        public void SetHealthValue(float health)
        {
            _currentHealth = health;
            HealthChanged?.Invoke(_currentHealth);
        }

        public void TakeDamage(float damage)
        {
            damage         -= _playerStats.Armor.GetValue();
            damage         =  Mathf.Clamp(damage, 1, int.MaxValue);
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0 && !_isDead)
            {
                _animator.SetTrigger(EAnimTriggers.Die.ToStringCached());
                _isDead = true;
                Died?.Invoke();
            }
            else if (_currentHealth > 0)
                ShowDamageEffect();
        }

        private async void ShowDamageEffect()
        {
            ChangeColor(Color.red);
            await UniTask.Delay(Constants.PlayerDamageTime);
            ChangeColor(Color.white);
        }

        private async void WaitWhileReloading(int milSec)
        {
            await UniTask.Delay(milSec);
            ReloadFinished();
        }

        private void ChangeColor(Color color)
        {
            foreach (var renderer in _renderers)
                renderer.material.color = color;
        }

        private void OnAmmoChanged(int ammo)          => AmmoChanged?.Invoke(ammo);
        private void OnMaxAmmoChanged(int maxAmmo)    => MaxAmmoChanged?.Invoke(maxAmmo);
        private void OnWeaponChanged(int weaponIndex) => WeaponChanged?.Invoke(weaponIndex);

        #endregion

        #region Movement

        public void Move(Vector3 direction)
        {
            if (_isDead)
                return;

            _animator.SetBool(EAnimTriggers.IsMoving.ToStringCached(), true);
            _rigidBody.MovePosition(transform.position + direction * _playerStats.MoveSpeed.GetValue() * Time.fixedDeltaTime);
        }

        public void Stop() => _animator.SetBool(EAnimTriggers.IsMoving.ToStringCached(), false);

        public void Rotate(Vector3 pointToRotate)
        {
            if (_isDead)
                return;

            var targetPosition = new Vector3(pointToRotate.x,
                transform.position.y,
                pointToRotate.z);
            transform.LookAt(targetPosition);
        }

        #endregion
    }
}