using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;
namespace Unity.FPS.UI
{
    public class ScopeUIManager : MonoBehaviour
    {
        #region Variables
        //����
        public GameObject scopeUI;
        private PlayerWeaponsManager weaponsManager;
        #endregion


        void Start()
        {
            //����
            weaponsManager = GameObject.FindObjectOfType<PlayerWeaponsManager>();
            //�̺�Ʈ �Լ� ���
            weaponsManager.OnScopedWeapon += OnScope;
            weaponsManager.OffScopedWeapon += OffScope;
        }


        public void OnScope()
        {
            scopeUI.SetActive(true);
            //Debug.Log("�ߵ�");
        }

        public void OffScope()
        {
            scopeUI.SetActive(false);
        }
    }
}