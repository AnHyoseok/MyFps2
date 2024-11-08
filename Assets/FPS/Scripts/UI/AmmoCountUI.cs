using TMPro;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    /// <summary>
    /// WeaponController ������ Ammo ī��Ʈ UI
    /// </summary>
    public class AmmoCountUI : MonoBehaviour
    {
        #region Variables
        private PlayerWeaponsManager playerWeaponsManager;

        private WeaponController weaponController;
        private int weaponIndex;

        //UI
        public TextMeshProUGUI weaponIndexText;

        public Image ammoFillImage;                 //Ammo rate�� ���� ������

        [SerializeField] private float ammoFillSharpness = 10f;     //������ ä���(����) �ӵ�
        [SerializeField] private float weaponSwitchSharpness = 10f;       //���� ��ü�� Ui�� �ٲ�� �ӵ�

        public CanvasGroup canvasGroup;
        [SerializeField][Range(0, 1)] private float unSelectedOpacity = 0.5f;
        private Vector3 unSelectedScale = Vector3.one * 0.8f;

        //�������� �� ����
        public FillBarColorChange fillBarColorChange;
        #endregion

        //AmmoCont UI �� �ʱ�ȭ
        public void Initialize(WeaponController weapon, int _weaponIndex)
        {
            weaponController = weapon;
            weaponIndex = _weaponIndex;

            //�����ε���
            weaponIndexText.text = (weaponIndex + 1).ToString();

            //�������� �� �ʱ�ȭ
            fillBarColorChange.Initialize(1f, 0.1f);

            //����
            playerWeaponsManager = GameObject.FindObjectOfType<PlayerWeaponsManager>();
        }

        private void Update()
        {
            float currentFillRate = weaponController.CurrentAmoRatio;
            ammoFillImage.fillAmount = Mathf.Lerp(ammoFillImage.fillAmount, currentFillRate, ammoFillSharpness * Time.deltaTime);

            //��Ƽ�� ���� ����
            bool isActiveWeapon = weaponController == playerWeaponsManager.GetActiveWeapon();
            float currentOpacity = isActiveWeapon ? 1.0f : unSelectedOpacity;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, currentOpacity, weaponSwitchSharpness * Time.deltaTime);
            Vector3 currentScale = isActiveWeapon ? Vector3.one : unSelectedScale;
            transform.localScale = Vector3.Lerp(transform.localScale, currentScale, weaponSwitchSharpness * Time.deltaTime);

            //���� ����
            fillBarColorChange.UpdateVisual(currentFillRate);
        }
    }
}