using Assets.BackToSchool.Scripts.Constants;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rayCastLenght = 100f;

        private Rigidbody _rigidBody;
        private Animator _animator;

        private Vector3 _direction = Vector3.zero;
        private Ray _ray;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            _direction.x = Input.GetAxisRaw("Horizontal");
            _direction.z = Input.GetAxisRaw("Vertical");

            _ray = _camera.ScreenPointToRay(Input.mousePosition);
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (Physics.Raycast(_ray, out var rayCastHit, _rayCastLenght, _layerMask))
            {
                transform.LookAt(rayCastHit.point);
            }

            if (_direction != Vector3.zero)
            {
                _animator.SetBool(AnimationStates.IsMoving, true);
                _rigidBody.MovePosition(transform.position + _direction * _moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                _animator.SetBool(AnimationStates.IsMoving, false);
            }
        }
    }
}