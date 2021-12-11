using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Components;
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

        public IPlayerController CreatePlayer(IPlayerInput playerInput, IResourceManager resourceManager, PlayerStats playerStats,
            PlayerData playerData);

        public IEnemySpawner CreateEnemySpawner();

        public IEnemy CreateEnemy(EEnemyTypes enemyType);

        public IBullet CreateFireBall();

        public IBullet CreateBullet(Transform position);

        public IBullet CreateRocket(Transform shootingTransform);

        public GameObject CreateMuzzleFlash(Transform shootingTransform);

        public GameObject CreateExplosion(Transform explosionTransform);

        public List<IWeapon> CreateAllWeapons(List<EWeapons> weaponsToCreate, Transform weaponTransform, Transform parenTransform);
    }
}