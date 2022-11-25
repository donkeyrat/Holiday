using UnityEngine;
using System.Collections.Generic;

namespace Holiday
{
    public class HolidayMaterialEvent : MonoBehaviour
    {
        public void ChangeMaterial()
        {
            var newMats = new List<Material>();
            foreach (var mat in rend.materials)
            {
                if (mat != rend.materials[index])
                {
                    newMats.Add(mat);
                }
                else
                {
                    newMats.Add(newMat);
                }
            }
            rend.materials = newMats.ToArray();
        }

        public Renderer rend;

        public int index;

        public Material newMat;
    }
}
