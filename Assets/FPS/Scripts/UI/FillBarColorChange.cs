using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
namespace Unity.FPS.UI
{
    /// <summary>
    /// 게이지바의 게이지색, 백그라운드색 변경
    /// </summary>
    public class FillBarColorChange : MonoBehaviour
    {
        #region Variables
        public Image foregroundImage;
        public Color defaultForeGroundColor; // 게이지의 기본 컬러
        public Color flashForegroundColorFull;          //게이지가 가득 차는 순간의 플래쉬 효과

        public Image backgroundImage;
        public Color defaultBackgroundColor;            //백그라운드 기본 컬러
        public Color flashBackgroundColorEmpty;         //게이지가 비는 순간의 플래쉬 효과

        private float fullValue = 1f;                   //게이지가 가득차있을때의 값
        private float emptyValue = 0f;                  //게이지가 비어있을때의 값

        [SerializeField] private float colorChangeSharpness = 5f;        //컬러 변경시 속도
        private float previousValue;                    //게이지가 풀로 차는 순간을 찾는 변수
        #endregion

        //색 변경 관련 값 초기화
        public void Initialize(float fullValueRatio, float emptyValueRatio)
        {
            fullValue = fullValueRatio;
            emptyValue = emptyValueRatio;

            previousValue = fullValue;
        }

        public void UpdateVisual(float currentRatio)
        {
            //게이지가 풀로 차는순간
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