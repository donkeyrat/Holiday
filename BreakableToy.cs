using UnityEngine;
using System.Collections.Generic;

namespace Holiday 
{
    public class BreakableToy : MonoBehaviour 
    {
        private void Start()
        {
            ownData = GetComponent<DataHandler>();
            ownData.unit.WasDealtDamageAction += BreakPart;
        }

        private void Update() 
        {
            counter += Time.deltaTime;
        }

        public void BreakPart(float damage) 
        {
            if (ownData.health <= ownData.maxHealth * percentHealthRequirement && Random.value < breakChance && counter >= cooldown && damage >= damageThreshold)
            {
                counter = 0f;
                    
                var selectedPart = breakableParts[Random.Range(0, breakableParts.Count - 1)];
                selectedPart.AddComponent<Rigidbody>().mass = selectedPart.GetComponentInParent<Rigidbody>().mass;
                selectedPart.transform.SetParent(transform.root);
                    
                if (selectedPart.GetComponentInChildren<ParticleSystem>()) selectedPart.GetComponentInChildren<ParticleSystem>().Play();
                if (selectedPart.GetComponentInChildren<PlaySoundEffect>()) selectedPart.GetComponentInChildren<PlaySoundEffect>().Go();
                    
                breakableParts.Remove(selectedPart);
            }
        }
        
        private DataHandler ownData;

        public List<GameObject> breakableParts;
        
        public float percentHealthRequirement = 0.5f;

        public float damageThreshold = 100f;

        public float breakChance = 0.5f;

        private float counter;

        public float cooldown = 3f;
    }
}