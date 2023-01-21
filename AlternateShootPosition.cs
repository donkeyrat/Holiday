using System.Collections.Generic;
using UnityEngine;

namespace Holiday
{
    public class AlternateShootPosition : MonoBehaviour
    {
        public void Switch()
        {
            shootPosition.transform.localPosition = positions[index];
            index++;
            if (index == positions.Count) index = 0;
        }

        private int index;

        public Transform shootPosition;

        public List<Vector3> positions;
    }
}