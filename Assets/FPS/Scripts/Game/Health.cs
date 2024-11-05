using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 체력을 관리하는 클래스
    /// </summary>
    public class Health : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float maxHealth = 100f;     //최대 Hp
        public float CurrentHealth { get; private set; } //현재 Hp
        private bool isDeath;                           //죽음 체크

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction OnDie;

        public UnityAction<float> OnHeal;

        //체력 위험 경계율
       [SerializeField] private float criticalHealRatio = 0.3f;

        //무적
        public bool isInvincible { get; private set; }
        #endregion

        //힐 아이템을 먹을 수 있는지 체크
        public bool CanPickUp() => CurrentHealth < maxHealth;

        //UI HP 게이지 값
        public float GetRatio() => CurrentHealth / maxHealth;

        //위험체크
        public bool IsCritical() => GetRatio() <= criticalHealRatio;

        private void Start()
        {
            //초기화
            CurrentHealth = maxHealth;
            isInvincible = false;
        }

        //힐
        public void Heal(float amount)
        {
            float beforeHealth = CurrentHealth;
            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

            //real Heal 구하기
            float realHeal = CurrentHealth - beforeHealth;
            if (realHeal < 0f)
            {
                //힐 구현
                OnHeal?.Invoke(realHeal);
            }
        }


        //damageSource : 데미지를 주는 주체
        public void TakeDamage(float damage, GameObject damageSource)
        {
            //죽음체크
            if (isDeath) return;
            //무적체크
            if (isInvincible) return;


            float beforeHealth = CurrentHealth;
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

            //real Damage 구하기
            float realDamage = beforeHealth - CurrentHealth;

            if (realDamage > 0f)
            {
                //데미지 구현
                OnDamaged?.Invoke(realDamage, damageSource);
            }

            //죽음처리
            HandleDeath();
        }

        void HandleDeath()
        {
            if (isDeath) return;

            if (CurrentHealth <= 0f)
            {
                isDeath = true;

                //죽음구현
                OnDie?.Invoke();

            }
        }
    }
}
