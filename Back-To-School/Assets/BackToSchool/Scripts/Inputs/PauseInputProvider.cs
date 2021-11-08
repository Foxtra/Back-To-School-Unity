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

        public void ScrollInvoked(float scrollValue)          => throw new NotSupportedException();
        public void RotateInvoked(Vector3 position)           => throw new NotSupportedException();
        public void FireInvoked()                             => throw new NotSupportedException();
        public void DirectionChangeInvoked(Vector3 direction) => throw new NotSupportedException();
        public void ReloadInvoked()                           => throw new NotSupportedException();
        public void StartListeningInput()                     => throw new NotSupportedException();

        public void PauseListeningInput() => _isPaused = true;

        public void ContinueListeningInput() => _isPaused = false;
    }
}