using Landfall.TABS;
using UnityEngine;

namespace Holiday
{
    public class BreakIntoUnits : MonoBehaviour
    {
        private Unit OwnUnit;
        private bool Done;
    
        public UnitBlueprint topUnit;
        public UnitBlueprint middleUnit;
        public UnitBlueprint bottomUnit;

        public Rigidbody topPart;
        public Rigidbody middlePart;
        public Rigidbody bottomPart;
    
        private void Start()
        {
            OwnUnit = transform.root.GetComponent<Unit>();
            OwnUnit.data.healthHandler.willBeRewived = true;
        }

        public void BreakIntoParts()
        {
            if (!OwnUnit.data.healthHandler.willBeRewived || Done) return;
        
            var topUnitSpawned = topUnit.Spawn(topPart.position, topPart.rotation, OwnUnit.Team);
            var middleUnitSpawned = middleUnit.Spawn(middlePart.position, middlePart.rotation, OwnUnit.Team);
            var bottomUnitSpawned = bottomUnit.Spawn(bottomPart.position, bottomPart.rotation, OwnUnit.Team);
        
            OwnUnit.data.healthHandler.willBeRewived = false;
            OwnUnit.data.hasBeenRevived = true;
            Done = true;
        }
    }
}
