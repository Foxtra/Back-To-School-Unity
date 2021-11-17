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
            if (!Input.GetButtonDown("Cancel"))
                return;
            foreach (var provider in _providers)
                provider.CancelInvoked();
        }

        private void CheckReload()
        {
            if (!Input.GetButtonDown("Reload"))
                return;
            foreach (var provider in _providers)
                provider.ReloadInvoked();
        }

        private void CheckFire()
        {
            if (!Input.GetButtonDown("Fire1"))
                return;
            foreach (var provider in _providers)
                provider.FireInvoked();
        }

        private void CheckRotation()
        {
            _mousePosition = Input.mousePosition;

            foreach (var provider in _providers)
                provider.RotateInvoked(_mousePosition);
        }

        private void CheckDirection()
        {
            _direction.x = Input.GetAxisRaw("Horizontal");
            _direction.z = Input.GetAxisRaw("Vertical");

            foreach (var provider in _providers)
                provider.DirectionChangeInvoked(_direction);
        }

        private void CheckMouseScroll()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            foreach (var provider in _providers)
                provider.ScrollInvoked(scroll);
        }
    }
}