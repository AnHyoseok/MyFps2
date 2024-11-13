using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
namespace Unity.FPS.AI
{
   public class EnemyController : MonoBehaviour
   {
       #region Variables
        private Health health;
        public GameObject deathVfxPrefab;
        public Transform deathVfxSpownPosition;
        #endregion

        private void Start()
        {
           //����
           health = GetComponent<Health>();
            health.OnDamaged += Ondamaged;
            health.OnDie += OnDie;
        }
        
        void Ondamaged(float damage, GameObject damageSource)
        {

        }

        void OnDie()
        {
            if(health.CurrentHealth <= 0)
            {
                //���� ȿ��
                GameObject effectGo = Instantiate(deathVfxPrefab, deathVfxSpownPosition.position, Quaternion.identity);
                Destroy(effectGo, 1f);
                
            }
        }

    }
}