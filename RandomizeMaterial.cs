using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holiday
{
    public class RandomizeMaterial : MonoBehaviour
    {
        private void Start()
        {
            rend = GetComponent<Renderer>();
            delay = Random.Range(minDelay, maxDelay);
            
            if (onStart)
            {
                ReplaceMaterials();
            }
        }

        public void ReplaceMaterials()
        {
            StopAllCoroutines();
            StartCoroutine(DoMatReplacing());
        }

        private IEnumerator DoMatReplacing()
        {
            yield return new WaitForSeconds(delay);

            rend.materials[index].CopyPropertiesFromMaterial(matsToReplaceWith[Random.Range(0, matsToReplaceWith.Count)]);
        }

        private Renderer rend;

        private float delay;
        
        public int index;

        public List<Material> matsToReplaceWith;

        public bool onStart;

        public float minDelay;

        public float maxDelay;
    }
}