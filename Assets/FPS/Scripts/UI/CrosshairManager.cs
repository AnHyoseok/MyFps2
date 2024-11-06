using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Gameplay;
namespace Unity.FPS.Game

{

   public class CrosshairManager : MonoBehaviour
   {
        #region Variables
        public Image crosshairImage;        //ũ�ν���� UI �̹���
        public Sprite nullCrosshairSprite;  //��Ƽ���� ���Ⱑ ������ 

        private RectTransform crosshairRectTransform;
        private CrossHairData crosshairDefault;         //����,�⺻
        private CrossHairData crosshairTarget;          //Ÿ���� �Ǿ�����

        private CrossHairData CrosshairCurrent;         //���������� �׸��� ũ�ν����
       [SerializeField] private float crosshairUpdateShrpness = 5.0f;   //Lerp ����

        private PlayerWeaponsManager weaponsManager;

        private bool wasPointingAtEnemy;
        #endregion

        private void Start()
        {
            //����
            weaponsManager = GameObject.FindObjectOfType<PlayerWeaponsManager>();
            //��Ƽ���� ���� ũ�ν� ��� ���̱�
            OnWeaponChanged(weaponsManager.GetActiveWeapon());

            weaponsManager.OnSwitchToWeapon += OnWeaponChanged;
        }

        private void Update()
        {
            UpdateCrosshairPointingAtEnemy(false);

            wasPointingAtEnemy = weaponsManager.IsPointingAtEnemy;
        }

        //ũ�ν� ��� �׸���
        void UpdateCrosshairPointingAtEnemy(bool force)
        {
            if(crosshairDefault.CrossHairSprite == null)
              return;

            if ((force|| wasPointingAtEnemy==false) && weaponsManager.IsPointingAtEnemy == true)
            {
                //�⺻����
                CrosshairCurrent = crosshairTarget;
            }
            else if((force|| wasPointingAtEnemy == true) && weaponsManager.IsPointingAtEnemy == false) 
            {
                //���� ����������
                CrosshairCurrent = crosshairDefault;
            }
            crosshairImage.sprite = CrosshairCurrent.CrossHairSprite;
            crosshairImage.color = Color.Lerp(crosshairImage.color, CrosshairCurrent.CrossHairColor, crosshairUpdateShrpness * Time.deltaTime);
            crosshairRectTransform.sizeDelta = Mathf.Lerp(crosshairRectTransform.sizeDelta.x, CrosshairCurrent.CrossHairSize, crosshairUpdateShrpness * Time.deltaTime) * Vector2.one;


        }

        //���Ⱑ �ٲ𶧸��� crosshairImage�� ������ ���� CrossHair�̹����ιٲٱ�
        void OnWeaponChanged(WeaponController newWeapon)
        {
            if(newWeapon) //���̾ƴϸ� !=null ������
            {
                crosshairImage.enabled = true;
                crosshairRectTransform = crosshairImage.GetComponent<RectTransform>();
                //��Ƽ�� ������ ũ�ν���� ���� ��������
                crosshairDefault = newWeapon.crosshairDefault;
                crosshairTarget = newWeapon.crosshairTargetInSight;

            }
            else
            {
                if (nullCrosshairSprite)
                {
                    crosshairImage.sprite = nullCrosshairSprite;
                }
                else
                {
                    crosshairImage.enabled = false;
                }
            }

            UpdateCrosshairPointingAtEnemy(true);
        }

        
    }

}