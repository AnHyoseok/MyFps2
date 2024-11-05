using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    /// <summary>
    /// ü���� �����ϴ� Ŭ����
    /// </summary>
    public class Health : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float maxHealth = 100f;     //�ִ� Hp
        public float CurrentHealth { get; private set; } //���� Hp
        private bool isDeath;                           //���� üũ

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction OnDie;

        public UnityAction<float> OnHeal;

        //ü�� ���� �����
       [SerializeField] private float criticalHealRatio = 0.3f;

        //����
        public bool isInvincible { get; private set; }
        #endregion

        //�� �������� ���� �� �ִ��� üũ
        public bool CanPickUp() => CurrentHealth < maxHealth;

        //UI HP ������ ��
        public float GetRatio() => CurrentHealth / maxHealth;

        //����üũ
        public bool IsCritical() => GetRatio() <= criticalHealRatio;

        private void Start()
        {
            //�ʱ�ȭ
            CurrentHealth = maxHealth;
            isInvincible = false;
        }

        //��
        public void Heal(float amount)
        {
            float beforeHealth = CurrentHealth;
            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

            //real Heal ���ϱ�
            float realHeal = CurrentHealth - beforeHealth;
            if (realHeal < 0f)
            {
                //�� ����
                OnHeal?.Invoke(realHeal);
            }
        }


        //damageSource : �������� �ִ� ��ü
        public void TakeDamage(float damage, GameObject damageSource)
        {
            //����üũ
            if (isDeath) return;
            //����üũ
            if (isInvincible) return;


            float beforeHealth = CurrentHealth;
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

            //real Damage ���ϱ�
            float realDamage = beforeHealth - CurrentHealth;

            if (realDamage > 0f)
            {
                //������ ����
                OnDamaged?.Invoke(realDamage, damageSource);
            }

            //����ó��
            HandleDeath();
        }

        void HandleDeath()
        {
            if (isDeath) return;

            if (CurrentHealth <= 0f)
            {
                isDeath = true;

                //��������
                OnDie?.Invoke();

            }
        }
    }
}
