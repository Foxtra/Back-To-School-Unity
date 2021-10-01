using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Player;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        public delegate void EnemyHandler(Enemy sender);

        public event EnemyHandler Notify;

        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _stopDistance = 1f;
        [SerializeField] private float _attackInterval = 2f;
        [SerializeField] private int _maxHealth = 2;

        private GameObject _player;
        private Animator _animator;
        private PlayerInteracting _playerInteracting;

        private float _damageTime = 1.2f;
        private float _deathTime = 1.5f;
        private float _timer;
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

            if (_currentHealth == 0 && _isDead)
            {
                EnemyDeath();
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
            _playerInteracting = _player.GetComponent<PlayerInteracting>();
        }

        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;
            if (!_isBusy && !_playerInteracting.IsDead)
            {
                if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _player.transform.position, _stopDistance))
                {
                    _animator.SetBool(AnimationStates.IsMoving, true);
                    transform.LookAt(_player.transform.position);
                    transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.fixedDeltaTime);
                }
                else
                {
                    _animator.SetBool(AnimationStates.IsMoving, false);

                    if (_timer > _attackInterval && !_playerInteracting.IsDead)
                    {
                        _isBusy = true;
                        Attack();
                        _timer = 0f;
                        Invoke(nameof(EnableEnemy), _attackInterval);
                    }
                }
            }
        }

        private void Attack()
        {
            _animator.SetTrigger(AnimationStates.Attack);
            _playerInteracting.GetDamage();
        }

        private void EnableEnemy()
        {
            _isBusy = false;
        }

        private void EnemyDeath()
        {
            Destroy(gameObject, _deathTime);
            Notify?.Invoke(this);
        }
    }
}