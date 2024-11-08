using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Gameplay;
using UnityEngine.UI;
using TMPro;
namespace Unity.FPS.Game
{
   public class PlayerUI : MonoBehaviour
   {
        #region Variables
        public TextMeshProUGUI bulletText;
        private PlayerWeaponsManager weaponsManager;
        #endregion

        private void Start()
        {
            //ÂüÁ¶
            weaponsManager = GameObject.FindObjectOfType<PlayerWeaponsManager>();
            weaponsManager.GetActiveWeapon();
        }
        private void Update()
        {
            VIewBulletCount();
        }


        void VIewBulletCount()
        {
            bulletText.text = $"{weaponsManager.GetActiveWeapon().weaponName}    \r\n  {weaponsManager.GetActiveWeapon().currentAmmo}";
        }
    }
}