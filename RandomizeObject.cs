using System.Collections.Generic;
using UnityEngine;

namespace Holiday
{
    public class RandomizeObject : MonoBehaviour
    {
        public void Awake()
        {
            if (GetComponent<ProjectileHit>()) hit = GetComponent<ProjectileHit>();
            if (GetComponentInParent<ProjectileHit>()) hit = GetComponentInParent<ProjectileHit>();
            
            if (randomizeOnAwake) Randomize();
        }
        
        public void Start()
        {
            if (randomizeOnStart && !randomizeOnAwake) Randomize();
        }

        public void Randomize()
        {
            var chosen = transform.GetChild(Random.Range(0, transform.childCount));
            chosen.gameObject.SetActive(true);
            if (addDelayEventListenersAsHitEvent && chosen.GetComponent<DelayEvent>())
            {
                var delayEvent = chosen.GetComponent<DelayEvent>();
                var hitEvents = new List<HitEvents>(hit.hitEvents)
                {
                    new HitEvents()
                    {
                        eventDelay = delayEvent.delay,
                        hitEvent = delayEvent.delayedEvent
                    }
                };
                hit.hitEvents = hitEvents.ToArray();
            }
        }

        private ProjectileHit hit;

        public bool randomizeOnStart;
        
        public bool randomizeOnAwake = true;

        public bool addDelayEventListenersAsHitEvent;
    }
}
