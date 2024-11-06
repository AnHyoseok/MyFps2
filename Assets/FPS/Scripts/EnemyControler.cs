using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Unity.FPS.Game
{
   public class EnemyControler : MonoBehaviour
   {
        #region Variables
        private Material _material;
        #endregion

        private void Awake()
        {
            _material = GetComponent<Material>();
        }
        private void Start()
        {
            Vector3 targetPos = new Vector3(-20.5f, 5f, 0f);
            transform.DOMove(targetPos, 20).SetLoops(-1, LoopType.Yoyo);
            transform.DOScale(new Vector3 (5,5,5), 20f).SetLoops(-1, LoopType.Yoyo);

        }
    }
}