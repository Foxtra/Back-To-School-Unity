using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Interfaces;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private float _moveSpeed = 5f;

        private Rigidbody _rigidBody;
        private Animator _animator;

        public void Move(Vector3 direction)
        {
            _animator.SetBool(AnimationStates.IsMoving, true);
            _rigidBody.MovePosition(transform.position + direction * _moveSpeed * Time.fixedDeltaTime);
        }

        public void Stop() => _animator.SetBool(AnimationStates.IsMoving, false);

        public void Rotate(Vector3 pointToRotate)
        {
            var targetPosition = new Vector3(pointToRotate.x,
                transform.position.y,
                pointToRotate.z);
            transform.LookAt(targetPosition);
        }

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _animator  = GetComponentInChildren<Animator>();
        }
    }
}