using UnityEngine;
using Landfall.TABS;
using System.Collections.Generic;

namespace Holiday
{
    public class BoulderBehavior : MonoBehaviour
    {
        public void OnCollisionEnter(Collision col)
        {
            var enemy = col.transform.root.GetComponent<Unit>();
            if (!disabled && col.collider.attachedRigidbody && enemy && GetComponent<TeamHolder>() && enemy.Team != GetComponent<TeamHolder>().team)
            {
                if (!hitList.Contains(enemy))
                {
                    enemy.data.healthHandler.TakeDamage(damage, Vector3.zero);
                    hitList.Add(enemy);
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
            }
            disabled = true;
        }

        private readonly List<Unit> hitList = new List<Unit>();

        private readonly List<Rigidbody> rigidList = new List<Rigidbody>();

        private readonly List<ConfigurableJoint> jointList = new List<ConfigurableJoint>();

        public float damage = 200f;

        private bool disabled;
    }
}
