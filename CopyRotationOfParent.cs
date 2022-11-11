using UnityEngine;

namespace Holiday
{
    public class CopyRotationOfParent : MonoBehaviour
    {
        public void Start()
        {
            transform.rotation = transform.parent.parent.rotation;
        }
    }
}
