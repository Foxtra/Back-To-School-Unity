using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Enums;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Parameters
{
    public static class Constants
    {
        public class EnemySpawn
        {
            public static float MinXpos = -49f;
            public static float MaxXpos = 49f;
            public static float MinZpos = -49f;
            public static float MaxZpos = 49f;
        }

        public class Camera
        {
            public static float RayCastLength = 100f;
            public static float CameraSmoothSpeed = 0.125f;
            public static Vector3 PlayerCameraOffset = new Vector3(0, 15, 0);
        }

        public class Time
        {
            public static int GameOverDelay = 1000;
            public static int TimerInvokeInterval = 1000;
            public static float MillisecondsMultiplier = 1000f;
        }

        public class EnemyStats
        {
            public static int EnemyMoveSpeed = 2;
            public static int ExperienceForEnemy = 20;
            public static int MaxWarriorEnemies = 3;
            public static int MaxShamanEnemies = 3;
            public static float MaxRangeToPlayer = 10f;
            public static float SpawnInterval = 1f;
            public static float ShamanAttackInterval = 2f;
            public static float EnemyStartChasingDistance = 15f;
            public static int EnemyWarriorMaxHealth = 2;
            public static int EnemyShamanMaxHealth = 1;
            public static int EnemyWarriorDamage = 2;
            public static int EnemyShamanDamage = 3;
            public static int EnemyFireBallForce = 4;
        }

        public class WeaponStats
        {
            public static int InitialFireRate = 1;
            public static int InitialMaxAmmo = 1;
            public static int InitialReloadSpeed = 1;

            public static int RifleInitialFireRate = 1;
            public static int RifleInitialMaxAmmo = 10;
            public static int RifleInitialReloadSpeed = 1;

            public static float BulletLifeTime = 4f;
            public static float BulletForce = 20f;
            public static float RocketForce = 25f;
        }

        public class PlayerStats
        {
            public static int InitialArmor = 0;
            public static int InitialDamage = 1;
            public static int InitialMaxHealth = 5;
            public static int InitialMoveSpeed = 4;

            public static Dictionary<EPlayerStats, int[]> PlayerProgression = new Dictionary<EPlayerStats, int[]>
            {
                [EPlayerStats.Armor]     = new[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                [EPlayerStats.Damage]    = new[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                [EPlayerStats.MaxHealth] = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10 },
                [EPlayerStats.MoveSpeed] = new[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            public static int[] ExperienceToLevelUpPerLevel = { 100, 120, 140, 160, 180, 200, 220, 250, 300, 400 };

            public static int PlayerDamageTime = 100;
        }

        public class Objectives
        {
            public static float TimeToSurvive = 120f;
            public static int WarriorEnemiesToKill = 2;
            public static int ShamanEnemiesToKill = 2;
        }
    }
}