using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ex
{
    public class TobniForce : MonoBehaviour
    {
        #region Variables
        public float speed = 100f;

        #endregion

        private void Update()
        {
            gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.back * speed, ForceMode.Impulse);

        }

    }
}