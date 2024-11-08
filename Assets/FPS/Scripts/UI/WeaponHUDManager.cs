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
        public RectTransform ammoPanel;     //ammoCountUI 부모 오브젝트
        public GameObject ammoCountPrefab;  //ammoCountUI 프리팹
        private PlayerWeaponsManager weaponsManager;
        #endregion

        private void Awake()
        {
            //참조
            weaponsManager = GameObject.FindObjectOfType<PlayerWeaponsManager>();
            weaponsManager.OnAddedWeapon += AddWeapon;
            weaponsManager.OnRemoveWeapon += RemoveWeapon;
            weaponsManager.OnSwitchToWeapon += SwitchWeapon;
        }


        //무기 추가 하면 ammoUI추가
        void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            GameObject ammoCountGo = Instantiate(ammoCountPrefab, ammoPanel);
            AmmoCountUI ammoCount = ammoCountGo.GetComponent<AmmoCountUI>();
            ammoCount.Initialize(newWeapon, weaponIndex);


        }

        //무기 제거 하면 ammoUI 제거
        void RemoveWeapon(WeaponController oldWeapon, int weaponIndex)
        {
            //제거
        }

        void SwitchWeapon(WeaponController weaponController)
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(ammoPanel);
        }
    }
}