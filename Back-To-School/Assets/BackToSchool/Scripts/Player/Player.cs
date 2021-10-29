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
        public event Action<float> HealthChanged;
        public event Action Died;

        public Animator Animator;
        public Inventory Inventory;
        public LevelSystem LevelSystem;
        public PlayerStats PlayerStats;
        public WeaponController WeaponController;

        [SerializeField] private float _damageTime = 0.1f;
        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private Rigidbody _rigidBody;
        private SkinnedMeshRenderer[] _renderers;

        private float _currentHealth;
        private bool _isDead;

        private void Awake()
        {
            _rigidBody       = GetComponent<Rigidbody>();
            Animator         = GetComponent<Animator>();
            _renderers       = GetComponentsInChildren<SkinnedMeshRenderer>();
            WeaponController = GetComponent<WeaponController>();
            Inventory        = GetComponent<Inventory>();
            WeaponController.SetInventory(Inventory);
            WeaponController.SetPlayer(this);
        }

        #region Shooting

        public void ReloadFinished() => WeaponController.ReloadComplete();

        public void Fire() => WeaponController.Shoot(PlayerStats.Damage.GetValue());

        #endregion

        #region Interaction

        public void InitializeHealth()
        {
            _currentHealth = PlayerStats.MaxHealth.GetValue();
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
            damage         -= PlayerStats.Armor.GetValue();
            damage         =  Mathf.Clamp(damage, 0, int.MaxValue);
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth);

            if (_currentHealth == 0 && !_isDead)
            {
                Animator.SetTrigger(AnimationStates.Die);
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
            Animator.SetBool(AnimationStates.IsMoving, true);
            _rigidBody.MovePosition(transform.position + direction * PlayerStats.MoveSpeed.GetValue() * Time.fixedDeltaTime);
        }

        public void Stop() => Animator.SetBool(AnimationStates.IsMoving, false);

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