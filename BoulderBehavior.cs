using UnityEngine;
using Landfall.TABS;
using System.Collections.Generic;

namespace Holiday
{
    public class BoulderBehavior : MonoBehaviour
    {
        private void Start()
        {
            rig = GetComponent<Rigidbody>();
            teamHolder = GetComponent<TeamHolder>();
        }
        
        public void OnCollisionEnter(Collision col)
        {
            var enemy = col.transform.root.GetComponent<Unit>();
            if (!disabled && col.rigidbody && col.rigidbody.mass * 10 < rig.mass && enemy && teamHolder && enemy.Team != teamHolder.team)
            {
                if (!enemy.GetComponent<HitByBoulder>())
                {
                    enemy.data.healthHandler.TakeDamage(damage, Vector3.zero);
                    enemy.gameObject.AddComponent<HitByBoulder>();
                    hitList.Add(enemy);
                }
                if (!col.gameObject.GetComponent<HitByBoulder>())
                {
                    var joint = col.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = rig;
                    joint.breakForce = breakForce;
                    joint.breakTorque = breakForce;
                    jointList.Add(joint);
                    
                    col.gameObject.AddComponent<HitByBoulder>();
                    rigList.Add(col.rigidbody);
                }

                col.rigidbody.velocity *= 0.3f;
                col.rigidbody.angularVelocity *= 0.3f;
            }
        }

        public void DestroyJoints()
        {
            foreach (var joint in jointList)
            {
                Destroy(joint);
            }
            jointList.Clear();

            foreach (var rig in rigList)
            {
                if (rig && rig.GetComponent<HitByBoulder>())
                {
                    rig.velocity *= 0f;
                    rig.angularVelocity *= 0f;
                    Destroy(rig.GetComponent<HitByBoulder>());
                }
            }
            rigList.Clear();

            foreach (var unit in hitList)
            {
                if (unit.GetComponent<HitByBoulder>()) Destroy(unit.GetComponent<HitByBoulder>());
            }
            hitList.Clear();
            
            disabled = true;
        }

        private Rigidbody rig;

        private TeamHolder teamHolder;

        private List<FixedJoint> jointList = new List<FixedJoint>();

        private List<Rigidbody> rigList = new List<Rigidbody>();

        private List<Unit> hitList = new List<Unit>();
        
        private bool disabled;

        public float damage = 200f;

        public float breakForce = 30000f;
        
        public class HitByBoulder : MonoBehaviour { }
    }
}
