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

        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerShooting = _player.GetComponent<PlayerShooting>();

            _playerShooting.OnAmmoChanged += PlayerAmmoInfo_OnAmmoChanged;
        }

        private void PlayerAmmoInfo_OnAmmoChanged(object sender, PlayerAmmoArgs _args)
        {
            UpdateAmmoText(_args.NewAmmoValue, _args.MaxAmmoValue);
        }

        private void UpdateAmmoText(int newAmmoValue, int maxAmmoValue)
        {
            _text.text = $"{newAmmoValue} / {maxAmmoValue}";
        }
    }
}