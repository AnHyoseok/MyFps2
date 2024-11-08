using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
namespace Unity.FPS.UI
{
    /// <summary>
    /// ���������� ��������, ��׶���� ����
    /// </summary>
    public class FillBarColorChange : MonoBehaviour
    {
        #region Variables
        public Image foregroundImage;
        public Color defaultForeGroundColor; // �������� �⺻ �÷�
        public Color flashForegroundColorFull;          //�������� ���� ���� ������ �÷��� ȿ��

        public Image backgroundImage;
        public Color defaultBackgroundColor;            //��׶��� �⺻ �÷�
        public Color flashBackgroundColorEmpty;         //�������� ��� ������ �÷��� ȿ��

        private float fullValue = 1f;                   //�������� �������������� ��
        private float emptyValue = 0f;                  //�������� ����������� ��

        [SerializeField] private float colorChangeSharpness = 5f;        //�÷� ����� �ӵ�
        private float previousValue;                    //�������� Ǯ�� ���� ������ ã�� ����
        #endregion

        //�� ���� ���� �� �ʱ�ȭ
        public void Initialize(float fullValueRatio, float emptyValueRatio)
        {
            fullValue = fullValueRatio;
            emptyValue = emptyValueRatio;

            previousValue = fullValue;
        }

        public void UpdateVisual(float currentRatio)
        {
            //�������� Ǯ�� ���¼���
            if (currentRatio == fullValue && currentRatio != previousValue)
            {
                foregroundImage.color = flashForegroundColorFull;

            }
            else if (currentRatio < emptyValue)
            {
                backgroundImage.color = flashBackgroundColorEmpty;
            }
            else
            {
                foregroundImage.color = Color.Lerp(foregroundImage.color, defaultForeGroundColor, colorChangeSharpness * Time.deltaTime);

                backgroundImage.color = Color.Lerp(backgroundImage.color, defaultBackgroundColor, colorChangeSharpness * Time.deltaTime);
            }

            previousValue = currentRatio;
            
        }

        
    }
}