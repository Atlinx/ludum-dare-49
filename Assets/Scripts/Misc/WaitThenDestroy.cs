using System;
using UnityEngine;

namespace Misc
{
    public class WaitThenDestroy : MonoBehaviour
    {
        public float LifespanSeconds = 1.0f;

        private float ticker = 0.0f;
        private void Update()
        {
            ticker += Time.deltaTime;

            if (ticker >= LifespanSeconds)
            {
                Destroy(this.gameObject);
            }
        }
    }
}