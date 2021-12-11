using System;
using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Interfaces.Game;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Player;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.UI;
using Assets.BackToSchool.Scripts.Weapons;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class ResourceManager : IResourceManager, ISystemResourceManager
    {
        public List<IWeapon> Weapons { get; private set; }

        public Camera CreateCamera(EGame type)
        {
            var result = CreatePrefabInstance<Camera, EGame>(type);
            return result;
        }

        public IInputManager CreateInputManager()
        {
            var result = CreatePrefabInstance<IInputManager, EGame>(EGame.InputManager);
            return result;
        }

        public IUIRoot CreateUIRoot(Camera worldSpaceCamera)
        {
            var result = CreatePrefabInstance<UIRoot, EViews>(EViews.UIRoot);
            result.Initialize(worldSpaceCamera);
            return result;
        }

        public IPlayerController CreatePlayer(IPlayerInput playerInput, IResourceManager resourceManager, PlayerStats playerStats,
            PlayerData playerData)
        {
            var result = CreatePrefabInstance<PlayerController, EGame>(EGame.Player);
            result.Initialize(playerInput, resourceManager, playerStats, playerData);
            return result;
        }

        public IEnemySpawner CreateEnemySpawner()
        {
            var result = CreatePrefabInstance<EnemySpawner, EGame>(EGame.EnemySpawner);
            return result;
        }

        public IEnemy CreateEnemy(EEnemyTypes enemyType)
        {
            IEnemy result;
            switch (enemyType)
            {
                case EEnemyTypes.EnemyWarrior:
                    result = CreatePrefabInstance<EnemyWarrior, EGame>(EGame.EnemyWarrior);
                    return result;
                case EEnemyTypes.EnemyShaman:
                    result = CreatePrefabInstance<EnemyShaman, EGame>(EGame.EnemyShaman);
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, null);
            }
        }

        public IBullet CreateFireBall()
        {
            var result = CreatePrefabInstance<BaseBullet, EGame>(EGame.FireBall);
            return result;
        }

        public IBullet CreateBullet(Transform shootingTransform)
        {
            var result = CreatePrefabInstance<BaseBullet, EGame>(EGame.Bullet);
            result.gameObject.transform.position = shootingTransform.position;
            result.gameObject.transform.rotation = shootingTransform.rotation;
            result.gameObject.transform.parent   = null;
            return result;
        }

        public IBullet CreateRocket(Transform shootingTransform)
        {
            var result = CreatePrefabInstance<Rocket, EGame>(EGame.Rocket);
            result.gameObject.transform.position = shootingTransform.position;
            result.gameObject.transform.rotation = shootingTransform.rotation;
            result.gameObject.transform.parent   = null;
            result.Initialize(this);
            return result;
        }

        public GameObject CreateMuzzleFlash(Transform shootingTransform)
        {
            var result = CreatePrefabInstance(EVFX.RocketMuzzle);
            result.gameObject.transform.position = shootingTransform.position;
            result.gameObject.transform.rotation = shootingTransform.rotation;
            return result;
        }

        public GameObject CreateExplosion(Transform explosionTransform)
        {
            var result = CreatePrefabInstance(EVFX.RocketExplosion);
            result.gameObject.transform.position = explosionTransform.position;
            result.gameObject.transform.parent   = null;
            return result;
        }

        public List<IWeapon> CreateAllWeapons(List<EWeapons> weaponsToCreate, Transform weaponTransform, Transform parenTransform)
        {
            Weapons = new List<IWeapon>();
            IWeapon result;
            WeaponStats stats;

            foreach (var weapon in weaponsToCreate)
            {
                switch (weapon)
                {
                    case EWeapons.AssaultRifle:
                        result = CreatePrefabInstance<AssaultRifle, EGame>(EGame.AssaultRifle);
                        stats = new WeaponStats(Constants.RifleInitialFireRate, Constants.RifleInitialMaxAmmo,
                            Constants.RifleInitialReloadSpeed);
                        break;
                    case EWeapons.RocketLauncher:
                        result = CreatePrefabInstance<RocketLauncher, EGame>(EGame.RocketLauncher);
                        stats = new WeaponStats(Constants.InitialFireRate, Constants.InitialMaxAmmo,
                            Constants.InitialReloadSpeed);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(weapon), weapon, null);
                }

                result.Initialize(stats, this, weaponTransform, parenTransform);
                Weapons.Add(result);
            }

            return Weapons;
        }

        public T GetPrefab<T, E>(E item)
            where T : Object
            where E : Enum
        {
            var path = $"{typeof(E).Name}/{item.ToStringCached()}";
            var result = Resources.Load<T>(path);
            return result;
        }

        public T CreatePrefabInstance<T, E>(E item) where E : Enum
        {
            var prefab = CreatePrefabInstance(item);
            var result = prefab.GetComponent<T>();

            return result;
        }

        public GameObject CreatePrefabInstance<E>(E item) where E : Enum
        {
            var path = $"{typeof(E).Name}/{item?.ToStringCached()}";
            var asset = Resources.Load<GameObject>(path);
            var result = Object.Instantiate(asset);

            return result;
        }
    }
}