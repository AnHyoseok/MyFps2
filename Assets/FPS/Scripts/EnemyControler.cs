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
            transform.DOScale(new Vector3(5, 5, 5), 5f);

        }
    }
}