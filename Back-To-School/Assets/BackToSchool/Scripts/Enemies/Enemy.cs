using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _stopDistance = 1f;
        [SerializeField] private int _maxHealth = 2;

        private GameObject _player;
        private Animator _animator;

        private float _damageTime = 1.2f;
        private float _deathTime = 1.5f;
        private int _currentHealth;

        private bool _isBusy;
        private bool _isDead;

        public void GetDamage()
        {
            _currentHealth--;
            _isBusy = true;

            if (_currentHealth == 0 && !_isDead)
            {
                _animator.SetTrigger(AnimationStates.Die);
                _isDead = true;
            }
            else if (_currentHealth > 0)
            {
                _animator.SetTrigger(AnimationStates.GetDamage);
            }

            Invoke(nameof(EnableEnemy), _damageTime);
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void FixedUpdate()
        {
            if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _player.transform.position, _stopDistance) && !_isBusy)
            {
                _animator.SetBool(AnimationStates.IsMoving, true);
                transform.LookAt(_player.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.fixedDeltaTime);
            }
            else
            {
                _animator.SetBool(AnimationStates.IsMoving, false);
            }
        }

        private void EnableEnemy()
        {
            if (_isDead)
            {
                Destroy(gameObject, _deathTime);
            }
            else
            {
                _isBusy = false;
            }
        }
    }
}