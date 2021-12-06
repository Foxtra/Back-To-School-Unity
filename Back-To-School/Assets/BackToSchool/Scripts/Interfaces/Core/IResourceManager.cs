using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Game;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using Assets.BackToSchool.Scripts.Player;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Core
{
    public interface IResourceManager
    {
        public Camera CreateCamera(EGame type);

        public IInputManager CreateInputManager();

        public IUIRoot CreateUIRoot(Camera worldSpaceCamera);

        public IPlayerController CreatePlayer(IPlayerInput playerInput, IAudioManager audioManager, PlayerStats playerStats,
            PlayerData playerData);

        public IEnemySpawner CreateEnemySpawner();
    }
}