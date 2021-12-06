using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using Assets.BackToSchool.Scripts.Parameters;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class HUDPresenter : MonoBehaviour, IHUDPresenter
    {
        [SerializeField] private GameObject _KillObjectives;
        [SerializeField] private GameObject _TimeObjectives;
        [SerializeField] private Text _warriorKillText;
        [SerializeField] private Text _shamanKillText;
        [SerializeField] private Text _timeRemainingText;

        [SerializeField] private Text _ammoText;
        [SerializeField] private Text _levelText;
        [SerializeField] private Text _armorText;
        [SerializeField] private Text _damageText;
        [SerializeField] private Text _moveSpeedText;
        [SerializeField] private Slider _heathBar;
        [SerializeField] private Slider _levelBar;
        [SerializeField] private Image _currentWeaponIcon;
        [SerializeField] private Sprite[] _weaponIcons;

        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private float _healthSliderValue;
        private float _currentHealth;
        private int _maxHealth;
        private int _ammoValue;
        private int _maxAmmoValue;

        public void SetRoot(RectTransform canvas) => transform.SetParent(canvas, false);

        public void ShowView() => gameObject.SetActive(true);
        public void HideView() => gameObject.SetActive(false);

        public void OnHealthChanged(float newCurrentHealth)
        {
            _currentHealth     = newCurrentHealth;
            _healthSliderValue = _currentHealth / _maxHealth;
            Invoke(nameof(UpdateHealthBar), _delayBeforeDamage);
        }

        public void OnMaxHealthChanged(int newMaxHealth)
        {
            _maxHealth         = newMaxHealth;
            _healthSliderValue = _currentHealth / _maxHealth;
            UpdateHealthBar();
        }

        public void OnWeaponChanged(int weaponNumber) { _currentWeaponIcon.sprite = _weaponIcons[weaponNumber]; }

        public void OnAmmoChanged(int newAmmoValue)
        {
            _ammoValue = newAmmoValue;
            UpdateAmmoText();
        }

        public void OnMaxAmmoChanged(int newMaxAmmoValue)
        {
            _maxAmmoValue = newMaxAmmoValue;
            UpdateAmmoText();
        }

        public void InitializeObjectives(ObjectiveParameters initializeParams)
        {
            switch (initializeParams.GameMode)
            {
                case EGameModes.KillEnemies:
                    _TimeObjectives.SetActive(false);
                    _KillObjectives.SetActive(true);
                    OnEnemiesKillChanged(initializeParams.WarriorEnemiesToKill - initializeParams.WarriorEnemiesKilled,
                        initializeParams.ShamanEnemiesToKill - initializeParams.ShamanEnemiesKilled);
                    break;
                case EGameModes.SurviveTime:
                    _KillObjectives.SetActive(false);
                    _TimeObjectives.SetActive(true);
                    OnTimeChanged(initializeParams.TimeToSurvive - initializeParams.SurvivedTime);
                    break;
            }
        }

        public void OnTimeChanged(float time)
        {
            var minutes = (int)Math.Floor(time / 60);
            var seconds = (int)(time % 60);
            minutes = Mathf.Clamp(minutes, 0, int.MaxValue);
            seconds = Mathf.Clamp(seconds, 0, int.MaxValue);
            UpdateRemainingTimeText(minutes, seconds);
        }

        public void OnEnemiesKillChanged(int warriorsToKill, int shamansToKill)
        {
            warriorsToKill = Mathf.Clamp(warriorsToKill, 0, int.MaxValue);
            shamansToKill  = Mathf.Clamp(shamansToKill, 0, int.MaxValue);
            UpdateWarriorKillText(warriorsToKill);
            UpdateShamanKillText(shamansToKill);
        }

        public void OnLevelChanged(int newLevel)       => UpdateLevelText(newLevel);
        public void OnExpChanged(float newSliderValue) => UpdateLevelBar(newSliderValue);

        public void OnArmorChanged(int newValue)     => UpdateArmorText(newValue);
        public void OnDamageChanged(int newValue)    => UpdateDamageText(newValue);
        public void OnMoveSpeedChanged(int newValue) => UpdateMoveSpeedText(newValue);

        private void UpdateHealthBar()                      => _heathBar.value = _healthSliderValue;
        private void UpdateLevelBar(float levelSliderValue) => _levelBar.value = levelSliderValue;

        private void UpdateAmmoText()                        => _ammoText.text = $"{_ammoValue} / {_maxAmmoValue}";
        private void UpdateArmorText(int newValue)           => _armorText.text = $"{newValue}";
        private void UpdateDamageText(int newValue)          => _damageText.text = $"{newValue}";
        private void UpdateMoveSpeedText(int newValue)       => _moveSpeedText.text = $"{newValue}";
        private void UpdateLevelText(int level)              => _levelText.text = $"Level: {level}";
        private void UpdateWarriorKillText(int killedNumber) => _warriorKillText.text = $"Warriors to kill: {killedNumber}";
        private void UpdateShamanKillText(int killedNumber)  => _shamanKillText.text = $"Shamans to kill: {killedNumber}";

        private void UpdateRemainingTimeText(int minutes, int seconds) =>
            _timeRemainingText.text = $"Time Remaining: {minutes} : {seconds}";
    }
}