using UnityEngine;
using Landfall.TABS;
using System.Collections.Generic;

namespace Holiday
{
    public class BoulderBehavior : MonoBehaviour
    {
        public void OnCollisionEnter(Collision col)
        {
            if (!disabled && col.collider.attachedRigidbody && col.collider.attachedRigidbody.transform.root.GetComponent<Unit>())
            {
                if (transform.root.GetComponent<Unit>() && col.collider.attachedRigidbody.transform.root.GetComponent<Unit>().Team == transform.root.GetComponent<Unit>().Team)
                {
                    return;
                }
                if (!hitList.Contains(col.collider.attachedRigidbody.transform.root.GetComponent<Unit>()))
                {
                    col.collider.attachedRigidbody.transform.root.GetComponent<Unit>().data.healthHandler.TakeDamage(damage, Vector3.zero);
                    hitList.Add(col.collider.attachedRigidbody.transform.root.GetComponent<Unit>());
                }
                if (!rigidList.Contains(col.collider.attachedRigidbody))
                {
                    jointList.Add(JointActions.AttachJoint(col.collider.attachedRigidbody, GetComponent<Rigidbody>(), 0f, 1000f, true, true, 1000f));
                    rigidList.Add(col.collider.attachedRigidbody);
                }
            }
        }

        public void DestroyJoints()
        {
            foreach (var joint in jointList)
            {
                Destroy(joint);
                disabled = true;
            }
        }

        private List<Unit> hitList = new List<Unit>();

        private List<Rigidbody> rigidList = new List<Rigidbody>();

        private List<ConfigurableJoint> jointList = new List<ConfigurableJoint>();

        public float damage = 200f;

        private bool disabled;
    }
}
