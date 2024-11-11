using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 크로스헤어를 그리기위한 데이터
    /// </summary>
    [System.Serializable]
    public struct CrossHairData
    {
        public Sprite CrossHairSprite;
        public float CrossHairSize;
        public Color CrossHairColor;
    }

    /// <summary>
    /// 무기 Shoot 타입
    /// </summary>
    public enum WeaponShootType
    {
        Manual,
        Automatic,
        Charge,
        Sniper,
    }


    /// <summary>
    /// 무기(총기)를 관리하는 클래스
    /// </summary>
    public class WeaponController : MonoBehaviour
    {
        #region Variables

        private PlayerInputHandler playerInputHandler;
        //무기 활성화, 비활성화
        public GameObject weaponRoot;

        public GameObject Owner { get; set; }   //무기의 주인
        public GameObject SourcePrefab { get; set; }    //무기를 생성한 오리지널 프리팹
        public bool IsWeaponActive { get; private set; }    //무기 활성화 여부

        public string weaponName; //무기이름

        private AudioSource shootAudioSource;
        public AudioClip switchWeaponSfx;

        //shooting
        public WeaponShootType shootType;

        public float maxAmmo = 8f;    //장전할 수 있는 최대 총알 갯수
        private float cuttentAmmo;
        [SerializeField] private float delayBetweenShots = 0.5f;    //슛간격
        private float lastTimeShot;                                 //마지막으로 슛한 시간

        //Vfx,Sfx
        public Transform weaponMuzzle;                  //총구 위치 
        public GameObject muzzleFlashPrefab;            //총구 발사 효과
        public AudioClip shootSfx;                      //총 발사 사운드

        //CrossHair
        public CrossHairData crosshairDefault;  //기본,평상시
        public CrossHairData crosshairTargetInSight;    //적을 포착했을때, 타켓팅 되었을때
        //조준
        public float aimZoomRatio = 1f; //조준시 줌인값
        public Vector3 aimOffset;       //조준시 무기 위치 조정값

        public float currentAmmo;

        //반동
        public float recoilForce = 0.5f;

        //Projectile
        public ProjectileBase projectilePrefab;

        public Vector3 MuzzleWorldVelocity { get; private set; }        //g현재 프레임에서의 총구시간
        private Vector3 lastMuzzlePosition;


        [SerializeField] private int bulletsPerShot = 1; //한번 슛하는데 발사되는 탄환의 개수
        [SerializeField] private float bulletSpreadAngle = 0f;    //불렛이 퍼져 나가는 각도

        //Charge : 발사 버튼을 누르고 있으면 발사체의 데미지,속도가 일정값까지 커진다
        public float CurrentCharge { get; private set; }
        public bool IsCharging { get; private set; }

        [SerializeField] private float ammoUseOnStartCharge = 1f; //충전 시작 버튼을 누르기 위해 필요한 ammo 량
        [SerializeField] private float ammoUsageRateWhileCharging = 1f; //차지하고 있는동안 소비되는 ammo량
        private float maxChargeDuration = 2f;   //충전시간 max

        public float lastChargeTriggerTimeStamp;    //충전 시작 시간
        #endregion
        public float CurrentAmoRatio => currentAmmo / maxAmmo;
        private void Awake()
        {
            //참조
            shootAudioSource = GetComponent<AudioSource>();

        }
        private void Start()
        {
            //초기화
            currentAmmo = maxAmmo;
            lastTimeShot = Time.time;
            lastMuzzlePosition = weaponMuzzle.position;
        }

        private void Update()
        {
            //충전
            UpdateCharge();


            //MuzzleWorldVelocity
            if (Time.time > 0)
            {
                MuzzleWorldVelocity = (weaponMuzzle.position - lastMuzzlePosition) / Time.deltaTime;

                lastMuzzlePosition = weaponMuzzle.position;
            }

            //치트키
            if (Input.GetKeyDown(KeyCode.T))
            {
                currentAmmo = maxAmmo;
            }

        }
        //충전
        void UpdateCharge()
        {
            if (IsCharging)
            {
                if (CurrentCharge < 1f)
                {
                    //현재 남아있는 충전량
                    float chargeLeft = 1f - CurrentCharge;

                    float chargeAdd = 0f; //이번 프레임에 충전할 량
                    if (maxChargeDuration <= 0f)
                    {
                        Debug.Log("chargeAdd"+chargeAdd);
                        chargeAdd = chargeLeft; //한번에 풀 충전
                    }
                    else
                    {
                        chargeAdd = (1f / maxChargeDuration) * Time.deltaTime;
                    }
                    chargeAdd = Mathf.Clamp(chargeAdd,0f,chargeLeft);   //남아있는 충전량보다 작아야한다 

                    //chargeAdd 만큼 탄환 소비
                    float ammoThisChargeRequire = chargeAdd * ammoUsageRateWhileCharging;
                    if(ammoThisChargeRequire <= currentAmmo)
                    {
                        UseAmmo(ammoThisChargeRequire);
                        CurrentCharge =Mathf.Clamp01(CurrentCharge + chargeAdd);
                    }
          
                }
            }
        }

        //무기 활성화, 비활성화
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);

            //this 무기로 변경
            if (show == true && switchWeaponSfx != null)
            {
                //무기 변경 효과음 플레이
                shootAudioSource.PlayOneShot(switchWeaponSfx);
            }

            IsWeaponActive = show;
        }

        //키 입력에 따른 슛 구현 
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
                        //충전시작
                        TryBeginCharge();
                    }
                    if (inputUp)
                    {
                        //충전 끝 발사
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

        //충전 시작
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

        //충전 끝 발사
        bool TryReleaseCharge()
        {
            if (IsCharging)
            {
                //슛
                HandleShoot();
                Debug.Log("충전끝발사    ");
                //초기화 
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
        //    //장전
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        currentAmmo = maxAmmo;
        //    }
        //}




        //슛연출
        void HandleShoot()
        {
            //project Tile 생성
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
            //마지막으로 슛한시간 저장
            lastTimeShot = Time.time;
        }

        //projectile 날아가는 방향
        Vector3 GetShotDirectionWithSpread(Transform shootTransfrom)
        {
            float spreadAngleRatio = bulletSpreadAngle / 180;
            return Vector3.Lerp(shootTransfrom.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);
        }


    }
}