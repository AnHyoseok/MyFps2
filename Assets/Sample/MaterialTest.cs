using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MySample
{
   public class MaterialTest : MonoBehaviour
   {
     

        private void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

            renderer.GetPropertyBlock(propBlock);

            propBlock.SetColor("_Color", Color.red);

            renderer.SetPropertyBlock(propBlock);
        }
    }
}