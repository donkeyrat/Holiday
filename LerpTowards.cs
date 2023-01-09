using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holiday
{
    public class LerpTowards : MonoBehaviour
    {
        public void Lerp()
        {
            if (target) StartCoroutine(DoLerping());
        }

        public IEnumerator DoLerping()
        {
            var t = 0f;
            var initialPosition = transform.position;
            while (t < 1f)
            {
                t += Time.deltaTime * lerpSpeed;
                transform.position = Vector3.Lerp(initialPosition, target.position, Mathf.Clamp(t, 0f, 1f));

                yield return null;
            }
        }

        public void SetTarget(Transform value)
        {
            target = value;
        }

        public Transform target;

        public float lerpSpeed = 1f;
    }
}