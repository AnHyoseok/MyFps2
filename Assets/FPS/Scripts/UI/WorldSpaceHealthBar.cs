using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UI;
namespace Unity.FPS.UI
{
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        #region Variables
        public Health health;
        public Camera mainCamera;
        public Image healthBarImage;
        public GameObject healthBarPivot;
   

        //Ǯ�̸� ������
        [SerializeField] private bool hideFullHealthBar = true;
        #endregion



        private void Update()
        {
            HIdeHealbar();
            healthBarImage.fillAmount = health.GetRatio();
            healthBarPivot.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back,
            mainCamera.transform.rotation * Vector3.up);

        }


        //hp�� Ǯ�̸� ü�¹� ������
        void HIdeHealbar()
        {
            if (hideFullHealthBar)
            {
                healthBarPivot.gameObject.SetActive(healthBarImage.fillAmount != 1f);
            }
        }


    }
}