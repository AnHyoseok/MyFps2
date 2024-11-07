using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// ũ�ν��� �׸������� ������
    /// </summary>
    [System.Serializable]
    public struct CrossHairData
    {
        public Sprite CrossHairSprite;
        public float CrossHairSize;
        public Color CrossHairColor;
    }

    /// <summary>
    /// ���� Shoot Ÿ��
    /// </summary>
    public enum WeaponShootType
    {
        Manual,
        Automatic,
        Charge,
        Sniper,
    }


    /// <summary>
    /// ����(�ѱ�)�� �����ϴ� Ŭ����
    /// </summary>
    public class WeaponController : MonoBehaviour
    {
        #region Variables

        private PlayerInputHandler playerInputHandler;
        //���� Ȱ��ȭ, ��Ȱ��ȭ
        public GameObject weaponRoot;

        public GameObject Owner { get; set; }   //������ ����
        public GameObject SourcePrefab { get; set; }    //���⸦ ������ �������� ������
        public bool IsWeaponActive { get; private set; }    //���� Ȱ��ȭ ����

        private AudioSource shootAudioSource;
        public AudioClip switchWeaponSfx;

        //shooting
        public WeaponShootType shootType;

        [SerializeField] private float maxAmmo = 8f;    //������ �� �ִ� �ִ� �Ѿ� ����
        private float cuttentAmmo;
        [SerializeField] private float delayBetweenShots = 0.5f;    //������
        private float lastTimeShot;                                 //���������� ���� �ð�

        //Vfx,Sfx
        public Transform weaponMuzzle;                  //�ѱ� ��ġ 
        public GameObject muzzleFlashPrefab;            //�ѱ� �߻� ȿ��
        public AudioClip shootSfx;                      //�� �߻� ����

        //CrossHair
        public CrossHairData crosshairDefault;  //�⺻,����
        public CrossHairData crosshairTargetInSight;    //���� ����������, Ÿ���� �Ǿ�����
        //����
        public float aimZoomRatio = 1f; //���ؽ� ���ΰ�
        public Vector3 aimOffset;       //���ؽ� ���� ��ġ ������

        public float currentAmmo;

        //�ݵ�
        public float recoilForce = 0.5f;

        //Projectile
        public ProjectileBase projectilePrefab;

        public Vector3 MuzzleWorldVelocity { get; private set; }
        private Vector3 lastMuzzlePosition;
        public float CurrentCharge { get; private set; }


        #endregion

        private void Awake()
        {
            //����
            shootAudioSource = GetComponent<AudioSource>();

        }
        private void Start()
        {
            //�ʱ�ȭ
            currentAmmo = maxAmmo;
            lastTimeShot = Time.time;
        }

        //���� Ȱ��ȭ, ��Ȱ��ȭ
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);

            //this ����� ����
            if (show == true && switchWeaponSfx != null)
            {
                //���� ���� ȿ���� �÷���
                shootAudioSource.PlayOneShot(switchWeaponSfx);
            }

            IsWeaponActive = show;
        }

        //Ű �Է¿� ���� �� ���� 
        public bool HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
        {


            switch (shootType)
            {
                case WeaponShootType.Manual:
                    if (inputDown)
                    {
                        Debug.Log("Manual");
                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Automatic:
                    if (inputHeld)
                    {
                        Debug.Log("Automatic");
                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Charge:
                    if (inputUp)
                    {

                    }
                    break;
                    case WeaponShootType.Sniper:
                    if (inputDown)
                    {
                        return TryShoot();
                    }
                    break;
            }
            return false;
        }

        bool TryShoot()
        {
            //
            if (currentAmmo >= 1f && (lastTimeShot + delayBetweenShots) < Time.time)
            {
                currentAmmo -= 1f;
                Debug.Log($"currentAmmo:{currentAmmo}");

                HandleShoot();
                return true;
            }
            return false;
        }

        //������
        void HandleShoot()
        {
            //Vfx
            if (muzzleFlashPrefab)
            {
                GameObject effectGo = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
                Destroy(effectGo, 1.5f);
            }

            //Sfx
            if (shootSfx)
            {
                shootAudioSource.PlayOneShot(shootSfx);
            }
            //���������� ���ѽð� ����
            lastTimeShot = Time.time;
        }
    }
}