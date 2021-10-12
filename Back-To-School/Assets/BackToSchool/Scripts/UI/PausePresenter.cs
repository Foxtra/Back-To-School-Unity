﻿using System;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class PausePresenter : MonoBehaviour
    {
        public event Action Restarted;
        public event Action Continued;

        [SerializeField] private Button _pauseRestartButton;
        [SerializeField] private Button _pauseContinueButton;

        public void TogglePausePanel(bool isPausePanelShowed) => gameObject.SetActive(isPausePanelShowed);

        private void Awake()
        {
            _pauseRestartButton.onClick.AddListener(Restart);
            _pauseContinueButton.onClick.AddListener(Continue);
        }

        private void Continue() => Continued?.Invoke();

        private void Restart() => Restarted?.Invoke();
    }
}