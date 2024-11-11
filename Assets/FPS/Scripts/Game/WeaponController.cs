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

        public string weaponName; //�����̸�

        private AudioSource shootAudioSource;
        public AudioClip switchWeaponSfx;

        //shooting
        public WeaponShootType shootType;

        public float maxAmmo = 8f;    //������ �� �ִ� �ִ� �Ѿ� ����
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

        public Vector3 MuzzleWorldVelocity { get; private set; }        //g���� �����ӿ����� �ѱ��ð�
        private Vector3 lastMuzzlePosition;


        [SerializeField] private int bulletsPerShot = 1; //�ѹ� ���ϴµ� �߻�Ǵ� źȯ�� ����
        [SerializeField] private float bulletSpreadAngle = 0f;    //�ҷ��� ���� ������ ����

        //Charge : �߻� ��ư�� ������ ������ �߻�ü�� ������,�ӵ��� ���������� Ŀ����
        public float CurrentCharge { get; private set; }
        public bool IsCharging { get; private set; }

        [SerializeField] private float ammoUseOnStartCharge = 1f; //���� ���� ��ư�� ������ ���� �ʿ��� ammo ��
        [SerializeField] private float ammoUsageRateWhileCharging = 1f; //�����ϰ� �ִµ��� �Һ�Ǵ� ammo��
        private float maxChargeDuration = 2f;   //�����ð� max

        public float lastChargeTriggerTimeStamp;    //���� ���� �ð�
        #endregion
        public float CurrentAmoRatio => currentAmmo / maxAmmo;
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
            lastMuzzlePosition = weaponMuzzle.position;
        }

        private void Update()
        {
            //����
            UpdateCharge();


            //MuzzleWorldVelocity
            if (Time.time > 0)
            {
                MuzzleWorldVelocity = (weaponMuzzle.position - lastMuzzlePosition) / Time.deltaTime;

                lastMuzzlePosition = weaponMuzzle.position;
            }

            //ġƮŰ
            if (Input.GetKeyDown(KeyCode.T))
            {
                currentAmmo = maxAmmo;
            }

        }
        //����
        void UpdateCharge()
        {
            if (IsCharging)
            {
                if (CurrentCharge < 1f)
                {
                    //���� �����ִ� ������
                    float chargeLeft = 1f - CurrentCharge;

                    float chargeAdd = 0f; //�̹� �����ӿ� ������ ��
                    if (maxChargeDuration <= 0f)
                    {
                        Debug.Log("chargeAdd"+chargeAdd);
                        chargeAdd = chargeLeft; //�ѹ��� Ǯ ����
                    }
                    else
                    {
                        chargeAdd = (1f / maxChargeDuration) * Time.deltaTime;
                    }
                    chargeAdd = Mathf.Clamp(chargeAdd,0f,chargeLeft);   //�����ִ� ���������� �۾ƾ��Ѵ� 

                    //chargeAdd ��ŭ źȯ �Һ�
                    float ammoThisChargeRequire = chargeAdd * ammoUsageRateWhileCharging;
                    if(ammoThisChargeRequire <= currentAmmo)
                    {
                        UseAmmo(ammoThisChargeRequire);
                        CurrentCharge =Mathf.Clamp01(CurrentCharge + chargeAdd);
                    }
          
                }
            }
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

                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Automatic:
                    if (inputHeld)
                    {

                        return TryShoot();
                    }
                    break;
                case WeaponShootType.Charge:
                    if (inputHeld)
                    {
                        //��������
                        TryBeginCharge();
                    }
                    if (inputUp)
                    {
                        //���� �� �߻�
                        return TryReleaseCharge();
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

        //���� ����
        void TryBeginCharge()
        {
            if (IsCharging == false && currentAmmo > ammoUseOnStartCharge && (lastTimeShot + delayBetweenShots) < Time.time)
            {
                UseAmmo(ammoUseOnStartCharge);

                lastChargeTriggerTimeStamp = Time.time;
                currentAmmo -= 1f;
                IsCharging = true;

            }
        }

        //���� �� �߻�
        bool TryReleaseCharge()
        {
            if (IsCharging)
            {
                //��
                HandleShoot();
                Debug.Log("�������߻�    ");
                //�ʱ�ȭ 
                CurrentCharge = 0f;
                IsCharging = false;
                return true;
            }

            return false;
        }

        void UseAmmo(float amount)
        {
            currentAmmo = Mathf.Clamp(currentAmmo - amount, 0f, maxAmmo);
            lastTimeShot = Time.time;
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

        //void Reload()
        //{
        //    //����
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        currentAmmo = maxAmmo;
        //    }
        //}




        //������
        void HandleShoot()
        {
            //project Tile ����
            for (int i = 0; i < bulletsPerShot; i++)
            {
                Vector3 ShotDirection = GetShotDirectionWithSpread(weaponMuzzle);
                ProjectileBase projectileBase = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(ShotDirection));
                projectileBase.Shoot(this);
            }
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

        //projectile ���ư��� ����
        Vector3 GetShotDirectionWithSpread(Transform shootTransfrom)
        {
            float spreadAngleRatio = bulletSpreadAngle / 180;
            return Vector3.Lerp(shootTransfrom.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);
        }


    }
}