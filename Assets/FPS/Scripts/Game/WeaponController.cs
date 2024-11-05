using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// ����(�ѱ�)�� �����ϴ� Ŭ����
    /// </summary>
   public class WeaponController : MonoBehaviour
   {
        #region Variables
        //���� Ȱ��ȭ, ��Ȱ��ȭ
        public GameObject weaponRoot;

        public GameObject Owner { get; set; }   //������ ����
        public GameObject SourcePrefab { get; set; }    //���⸦ ������ �������� ������
        public bool IsWeaponActive {  get;private set; }    //���� Ȱ��ȭ ����

        private AudioSource shootAudioSource;
        public AudioClip switchWeaponSfx;
        #endregion

        private void Awake()
        {
            //����
            shootAudioSource = GetComponent<AudioSource>();
        }

        //���� Ȱ��ȭ, ��Ȱ��ȭ
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);

            //this ����� ����
            if (show == true && switchWeaponSfx !=null)
            {
                //���� ���� ȿ���� �÷���
                shootAudioSource.PlayOneShot(switchWeaponSfx);
            }

            IsWeaponActive = show;
        }
   }
}