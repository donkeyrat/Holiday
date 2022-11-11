using UnityEngine;

namespace Holiday
{
    public class RandomizeObject : MonoBehaviour
    {
        public void Start()
        {
            Randomize();
        }

        public void Randomize()
        {
            var chosen = transform.GetChild(Random.Range(0, transform.childCount - 1));
            chosen.gameObject.SetActive(true);
        }
    }
}
