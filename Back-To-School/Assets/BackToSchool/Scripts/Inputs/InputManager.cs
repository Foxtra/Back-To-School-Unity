using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Inputs
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        private Vector3 _direction = Vector3.zero;
        private Vector3 _mousePosition;

        private List<IRawInputProvider> _providers = new List<IRawInputProvider>();

        public void Subscribe(IRawInputProvider provider) => _providers.Add(provider);

        public void Unsubscribe(IRawInputProvider provider) => _providers.Remove(provider);

        private void Update()
        {
            CheckDirection();
            CheckRotation();
            CheckFire();
            CheckReload();
            CheckCancel();
            CheckMouseScroll();
        }

        private void CheckCancel()
        {
            if (!UnityEngine.Input.GetButtonDown("Cancel"))
                return;
            foreach (var provider in _providers)
                provider.CancelInvoked();
        }

        private void CheckReload()
        {
            if (!UnityEngine.Input.GetButtonDown("Reload"))
                return;
            foreach (var provider in _providers)
                provider.ReloadInvoked();
        }

        private void CheckFire()
        {
            if (!UnityEngine.Input.GetButtonDown("Fire1"))
                return;
            foreach (var provider in _providers)
                provider.FireInvoked();
        }

        private void CheckRotation()
        {
            _mousePosition = UnityEngine.Input.mousePosition;

            foreach (var provider in _providers)
                provider.RotateInvoked(_mousePosition);
        }

        private void CheckDirection()
        {
            _direction.x = UnityEngine.Input.GetAxisRaw("Horizontal");
            _direction.z = UnityEngine.Input.GetAxisRaw("Vertical");

            foreach (var provider in _providers)
                provider.DirectionChangeInvoked(_direction);
        }

        private void CheckMouseScroll()
        {
            var scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            foreach (var provider in _providers)
                provider.ScrollInvoked(scroll);
        }
    }
}