using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// �������� �Դ� �浹ü(hit box) �� �����Ǿ� �������� �����ϴ� Ŭ����
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        #region Variables
        private Health health;
        //������ ���
        [SerializeField] private float damageMultiplier = 1f;

        //�ڽ��� ���� ������ ���
        [SerializeField] private float sensibilityToSelfDamage = 0.5f;
        #endregion

        private void Awake()
        {
            health = GetComponent<Health>();
            if (health == null)
            {
                health = GetComponentInParent<Health>();
            }
        }

        public void InflictDamage(float damage,bool isExplosionDamage ,GameObject damageSource)
        {
            if(health == null)   return;

            //totalDamage�� ���� ��������
            var totalDamage = damage;

            //���� ������ üũ - ���� �������϶���damageMultiplier�� ������� �ʴ´�
            if (isExplosionDamage == false)
            {
                totalDamage *= damageMultiplier;
            }

            //�ڽ��� ���� �������̸�
            if (damageSource == health.gameObject)
            {
                totalDamage *=sensibilityToSelfDamage;
            }

            //������ ������
            health.TakeDamage(totalDamage,damageSource);
        }
    }
}