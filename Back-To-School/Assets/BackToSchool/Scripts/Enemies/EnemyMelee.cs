using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyMelee : MonoBehaviour
    {
        public event Action PlayerDamaged;

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag(ETags.Player.ToStringCached()))
                PlayerDamaged?.Invoke();
        }
    }
}