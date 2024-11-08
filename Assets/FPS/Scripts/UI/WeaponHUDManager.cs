using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
namespace Unity.FPS.UI
{
    public class WeaponHUDManager : MonoBehaviour
    {
        #region Variables
        public RectTransform ammoPanel;     //ammoCountUI �θ� ������Ʈ
        public GameObject ammoCountPrefab;  //ammoCountUI ������
        private PlayerWeaponsManager weaponsManager;
        #endregion

        private void Awake()
        {
            //����
            weaponsManager = GameObject.FindObjectOfType<PlayerWeaponsManager>();
            weaponsManager.OnAddedWeapon += AddWeapon;
            weaponsManager.OnRemoveWeapon += RemoveWeapon;
            weaponsManager.OnSwitchToWeapon += SwitchWeapon;
        }


        //���� �߰� �ϸ� ammoUI�߰�
        void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            GameObject ammoCountGo = Instantiate(ammoCountPrefab, ammoPanel);
            AmmoCountUI ammoCount = ammoCountGo.GetComponent<AmmoCountUI>();
            ammoCount.Initialize(newWeapon, weaponIndex);


        }

        //���� ���� �ϸ� ammoUI ����
        void RemoveWeapon(WeaponController oldWeapon, int weaponIndex)
        {
            //����
        }

        void SwitchWeapon(WeaponController weaponController)
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(ammoPanel);
        }
    }
}