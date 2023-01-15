using UnityEngine;

namespace Holiday
{
    public class RandomizeObject : MonoBehaviour
    {
        public void Awake()
        {
            if (randomizeOnAwake) Randomize();
        }
        
        public void Start()
        {
            if (randomizeOnStart && !randomizeOnAwake) Randomize();
        }

        public void Randomize()
        {
            var chosen = transform.GetChild(Random.Range(0, transform.childCount));
            chosen.gameObject.SetActive(true);
        }

        public bool randomizeOnStart;
        
        public bool randomizeOnAwake = true;
    }
}
