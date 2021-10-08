﻿using Assets.BackToSchool.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        private Slider _slider;
        private GameObject _player;
        private PlayerInteracting _playerInteracting;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerInteracting = _player.GetComponent<PlayerInteracting>();

            _playerInteracting.OnHealthChanged += PlayerHealthBar_OnHealthChanged;
        }

        private void PlayerHealthBar_OnHealthChanged(object sender, PlayerHealthArgs args)
        {
            UpdateHealthBar(args.NewHealthValue);
        }

        private void UpdateHealthBar(float newValue)
        {
            _slider.value = newValue;
        }
    }
}