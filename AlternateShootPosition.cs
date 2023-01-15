using System.Collections.Generic;
using UnityEngine;

namespace Holiday
{
    public class AlternateShootPosition : MonoBehaviour
    {
        public void Switch()
        {
            shootPosition.transform.position = positions[index];
            index++;
        }

        private int index;

        public Transform shootPosition;

        public List<Vector3> positions;
    }
}