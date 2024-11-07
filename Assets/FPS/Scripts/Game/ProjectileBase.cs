using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.FPS.Gameplay;
namespace Unity.FPS.Game
{
    /// <summary>
    /// �߻�ü�� �⺻�� �Ǵ� �θ� Ŭ����
    /// </summary>
    public abstract class ProjectileBase : MonoBehaviour
    {
        #region Variables
        public GameObject Owner { get; private set; } // �߻��� ��ü 
        public Vector3 InitialPosition { get; private set; }    //�ʱ�������
        public Vector3 InitialDirection { get; private set; }   //�ʱ����
        public Vector3 InheritedMuzzleVelocity { get; private set; }    //�ѱ��ӵ�
        public float InitialCharge { get; private set; }    //�ʱ� ������

        public UnityAction OnShot;                          //�߻�� ��ϵ� �Լ� ȣ��
        #endregion

        public void Shoot(WeaponController controller)
        {
            //�ʱ�ȭ
            Owner = controller.Owner;
            InitialPosition = this.transform.position;
            InitialDirection = this.transform.forward;
            InheritedMuzzleVelocity = controller.MuzzleWorldVelocity;
            InitialCharge = controller.CurrentCharge;

            OnShot?.Invoke();
        }
    }
}