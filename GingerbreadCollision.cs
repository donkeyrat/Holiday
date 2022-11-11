using UnityEngine;
using UnityEngine.Events;

namespace Holiday
{
    public class GingerbreadCollision : MonoBehaviour
    {
        public void OnCollisionEnter(Collision col)
        {
            if (!col.collider.attachedRigidbody && !done)
            {
                collisionEvent.Invoke();
                done = true;
            }
        }

        public UnityEvent collisionEvent = new UnityEvent();

        private bool done;
    }
}
