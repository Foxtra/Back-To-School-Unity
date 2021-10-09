using Assets.BackToSchool.Scripts.Constants;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;

        private Rigidbody _rigidBody;
        private Animator _animator;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void Move(Vector3 direction)
        {
            _animator.SetBool(AnimationStates.IsMoving, true);
            _rigidBody.MovePosition(transform.position + direction * _moveSpeed * Time.fixedDeltaTime);
        }

        public void Stop()
        {
            _animator.SetBool(AnimationStates.IsMoving, false);
        }

        public void Rotate(RaycastHit rayCastHit)
        {
            transform.LookAt(rayCastHit.point);
        }
    }
}