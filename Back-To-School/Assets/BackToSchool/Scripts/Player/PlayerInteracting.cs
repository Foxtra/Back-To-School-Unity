using Assets.BackToSchool.Scripts.Constants;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerInteracting : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 5;
        [SerializeField] private float _damageTime = 0.1f;

        private SkinnedMeshRenderer[] _renderers;
        private Animator _animator;

        private int _currentHealth;
        public bool IsDead { get; private set; }

        public void GetDamage()
        {
            _currentHealth--;
            Debug.Log("My hp is " + _currentHealth);

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