using System;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Inputs
{
    internal class PauseInputProvider : IPauseInput
    {
        public event Action Cancelled;

        private bool _isPaused;

        public void CancelInvoked()
        {
            if (_isPaused) Cancelled?.Invoke();
        }

        public void ScrollInvoked(float scrollValue) { }

        public void RotateInvoked(Vector3 position) { }

        public void FireInvoked() { }

        public void DirectionChangeInvoked(Vector3 direction) { }

        public void ReloadInvoked() { }

        public void StartListeningInput() { }

        public void PauseListeningInput() => _isPaused = true;

        public void ContinueListeningInput() => _isPaused = false;
    }
}