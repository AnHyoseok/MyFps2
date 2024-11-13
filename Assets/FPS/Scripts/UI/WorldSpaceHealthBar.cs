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
   

        //풀이면 가리기
        [SerializeField] private bool hideFullHealthBar = true;
        #endregion



        private void Update()
        {
            HIdeHealbar();
            healthBarImage.fillAmount = health.GetRatio();
            healthBarPivot.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back,
            mainCamera.transform.rotation * Vector3.up);

        }


        //hp가 풀이면 체력바 가리기
        void HIdeHealbar()
        {
            if (hideFullHealthBar)
            {
                healthBarPivot.gameObject.SetActive(healthBarImage.fillAmount != 1f);
            }
        }


    }
}