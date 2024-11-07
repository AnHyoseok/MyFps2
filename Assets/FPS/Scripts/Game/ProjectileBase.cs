using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.FPS.Gameplay;
namespace Unity.FPS.Game
{
    /// <summary>
    /// 발사체의 기본이 되는 부모 클래스
    /// </summary>
    public abstract class ProjectileBase : MonoBehaviour
    {
        #region Variables
        public GameObject Owner { get; private set; } // 발사한 주체 
        public Vector3 InitialPosition { get; private set; }    //초기포지션
        public Vector3 InitialDirection { get; private set; }   //초기방향
        public Vector3 InheritedMuzzleVelocity { get; private set; }    //총구속도
        public float InitialCharge { get; private set; }    //초기 차지값

        public UnityAction OnShot;                          //발사시 등록된 함수 호출
        #endregion

        public void Shoot(WeaponController controller)
        {
            //초기화
            Owner = controller.Owner;
            InitialPosition = this.transform.position;
            InitialDirection = this.transform.forward;
            InheritedMuzzleVelocity = controller.MuzzleWorldVelocity;
            InitialCharge = controller.CurrentCharge;

            OnShot?.Invoke();
        }
    }
}