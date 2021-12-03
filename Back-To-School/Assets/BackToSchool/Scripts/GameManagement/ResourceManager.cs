﻿using System;
using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Interfaces.Game;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using Assets.BackToSchool.Scripts.Player;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.UI;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class ResourceManager : IResourceManager, ISystemResourceManager
    {
        public Camera CreateCamera(EGame type)
        {
            var result = CreatePrefabInstance<Camera, EGame>(type);
            return result;
        }

        public IUIRoot CreateUIRoot(Camera worldSpaceCamera)
        {
            var result = CreatePrefabInstance<UIRoot, EViews>(EViews.UIRoot);
            result.Initialize(worldSpaceCamera);
            return result;
        }

        public IPlayerController CreatePlayer(IPlayerInput playerInput, PlayerStats playerStats, PlayerData playerData)
        {
            var result = CreatePrefabInstance<PlayerController, EGame>(EGame.Player);
            result.Initialize(playerInput, playerStats, playerData);
            return result;
        }

        public IEnemySpawner CreateEnemySpawner()
        {
            var result = CreatePrefabInstance<EnemySpawner, EGame>(EGame.EnemySpawner);
            return result;
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