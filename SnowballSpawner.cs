using UnityEngine;
using Landfall.TABS;

namespace Holiday
{
    public class SnowballSpawner : MonoBehaviour
    {
        public void DoSpawn()
        {
            var self = transform.root.GetComponent<Unit>();
            if (objectToSpawn && self && self.data.targetMainRig)
            {
                var snowball = Instantiate(objectToSpawn, transform.position, Quaternion.LookRotation(new Vector3(self.data.targetData.mainRig.position.x - self.data.mainRig.position.x, 0f, self.data.targetData.mainRig.position.z - self.data.mainRig.position.z)));
                if (copyRotation) snowball.GetComponent<CopyRotationOfParent>().target.rotation = transform.rotation;

                TeamHolder.AddTeamHolder(snowball, gameObject);
            }
        }

        public GameObject objectToSpawn;

        public bool copyRotation;
    }
}