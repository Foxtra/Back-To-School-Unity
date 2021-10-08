using System;
using Assets.BackToSchool.Scripts.Constants;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerInteracting : MonoBehaviour
    {
        public delegate void PlayerInteractingHandler(PlayerInteracting sender, PlayerHealthArgs _args);

        public event PlayerInteractingHandler OnHealthChanged;

        [SerializeField] private int _maxHealth = 5;
        [SerializeField] private float _damageTime = 0.1f;

        private SkinnedMeshRenderer[] _renderers;
        private Animator _animator;

        private int _currentHealth;
        public bool IsDead { get; private set; }

        public void GetDamage()
        {
            _currentHealth--;
            if (OnHealthChanged != null) OnHealthChanged(this, new PlayerHealthArgs((float) _currentHealth / _maxHealth));

            if (_currentHealth == 0 && !IsDead)
            {
                _animator.SetTrigger(AnimationStates.Die);
                IsDead = true;
            }
            else if (_currentHealth > 0)
            {
                foreach (var renderer in _renderers)
                {
                    renderer.material.color = Color.red;
                }

                Invoke(nameof(ChangeColor), _damageTime);
            }
        }

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        private void ChangeColor()
        {
            foreach (var renderer in _renderers)
            {
                renderer.material.color = Color.white;
            }
        }
    }
}

public class PlayerHealthArgs : EventArgs
{
    public float NewHealthValue { get; }

    public PlayerHealthArgs(float newHealthValue)
    {
        NewHealthValue = newHealthValue;
    }
}