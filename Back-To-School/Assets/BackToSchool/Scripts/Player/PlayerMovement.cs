using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour

    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Camera _camera;

        private Rigidbody _rigidBody;

        private Vector3 _direction = Vector3.zero;
        private Vector2 _mousePosition;
        private Vector2 _cameraPosition;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()

        {
            _direction.x = Input.GetAxisRaw("Horizontal");
            _direction.z = Input.GetAxisRaw("Vertical");

            _cameraPosition = _camera.WorldToViewportPoint(transform.position);
            _mousePosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        }

        private void FixedUpdate()

        {
            _rigidBody.MovePosition(transform.position + _direction * _moveSpeed * Time.fixedDeltaTime);

            var angle = AngleBetweenTwoPoints(_cameraPosition, _mousePosition);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }

        private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(b.x - a.x, b.y - a.y) * Mathf.Rad2Deg;
        }
    }
}