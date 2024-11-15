using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
namespace Unity.FPS.AI
{
    /// <summary>
    /// ���� ������ ���� 
    /// </summary>
    public class DetectionModule : MonoBehaviour
    {
        #region Variables
        private ActorManager actorManager;

        public UnityAction OnDetectedTarget;   //���� �����ϸ� ��ϵ� �Լ� ȣ��
        public UnityAction OnLostTarget;        //���� ��ġ�� ��ϵ� �Լ� ȣ��
        #endregion


        private void Awake()
        {
            actorManager = GameObject.FindAnyObjectByType<ActorManager>();
        }

        //������
        public void HandleTargetDetection(Actor actor, Collider[] selfCollider)
        {
            
        }

        //���� �����ϸ�
        public void OnDetected()
        {
            OnDetectedTarget?.Invoke();
        }

        //���� ��ġ��
        public void OnLosted()
        {

            OnLostTarget?.Invoke();
        }
    }
}
}