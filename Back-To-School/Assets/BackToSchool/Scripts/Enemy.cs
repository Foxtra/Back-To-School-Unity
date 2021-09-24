using UnityEngine;


namespace Assets.BackToSchool.Scripts
{
    public class Enemy : MonoBehaviour
    {
        private Renderer _renderer;

        private float _damageTime = 0.1f;

        public void GetDamage()
        {
            _renderer.material.color = Color.red;
            Invoke(nameof(ChangeColor), _damageTime);
        }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void ChangeColor()
        {
            _renderer.material.color = Color.white;
        }
    }
}