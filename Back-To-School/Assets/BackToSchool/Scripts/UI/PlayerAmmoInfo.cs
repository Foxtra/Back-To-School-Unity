using Assets.BackToSchool.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class PlayerAmmoInfo : MonoBehaviour
    {
        private Text _text;
        private GameObject _player;
        private PlayerShooting _playerShooting;

        private void Awake() { _text = GetComponentInChildren<Text>(); }

        private void Start()
        {
            _player         = GameObject.FindGameObjectWithTag("Player");
            _playerShooting = _player.GetComponent<PlayerShooting>();

            _playerShooting.AmmoChanged += OnAmmoChanged;
        }

        private void OnAmmoChanged(int newAmmoValue, int maxAmmoValue) { UpdateAmmoText(newAmmoValue, maxAmmoValue); }

        private void UpdateAmmoText(int newAmmoValue, int maxAmmoValue) { _text.text = $"{newAmmoValue} / {maxAmmoValue}"; }
    }
}