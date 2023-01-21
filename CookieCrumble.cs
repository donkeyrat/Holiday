using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Landfall.TABS;
using Unity.Mathematics;
using UnityEngine.Events;

namespace Holiday
{
    public class CookieCrumble : ProjectileSurfaceEffect
    {
        private void Start()
        {
            ownUnit = transform.root.GetComponent<Unit>();
            health = maxHealth;
        }
        
        public override bool DoEffect(HitData hit, GameObject projectile)
        {
            if (!ownUnit || !ownUnit.data || !ownUnit.data.targetMainRig || currentState != CookieState.Default) return false;
            
            if (projectile.GetComponent<ProjectileHit>()) health -= projectile.GetComponent<ProjectileHit>().damage;
            if (projectile.GetComponent<CollisionWeapon>()) health -= projectile.GetComponent<CollisionWeapon>().damage;

            if (health <= 0f)
            {
                health = 0f;
                Crumble();
            }
            
            return true;
        }

        public void ChangeState(CookieState state = CookieState.Default)
        {
            currentState = state;
            switch (state)
            {
                case CookieState.Default:
                    defaultEvent.Invoke();
                    break;
                case CookieState.Crumbled:
                    crumbleEvent.Invoke();
                    foreach (var move in movesToStun) move.StunAllOfMyMovesFor(crumbleTime + 1f);
                    break;
                case CookieState.Pushing:
                    pushEvent.Invoke();
                    foreach (var move in movesToStun) move.StunAllOfMyMovesFor(pushEndDelay + 2f);
                    break;
                case CookieState.Crushing:
                    crushEvent.Invoke();
                    foreach (var move in movesToStun) move.StunAllOfMyMovesFor(crushEndDelay + 2f);
                    break;
                default:
                    defaultEvent.Invoke();
                    break;
            }
        }

        public void Crumble()
        {
            if (!ownUnit || !ownUnit.data || !ownUnit.data.targetMainRig || currentState != CookieState.Default) return;
            
            ChangeState(CookieState.Crumbled);
            StartCoroutine(DoCrumble());
        }

        private IEnumerator DoCrumble()
        {
            Instantiate(crumbleObject, transform.TransformPoint(modelOffset), transform.rotation);
            yield return new WaitForSeconds(crumbleTime);

            health = maxHealth;
            ChangeState();
        }

        public void Crush()
        {
            if (!ownUnit || !ownUnit.data || !ownUnit.data.targetMainRig || currentState != CookieState.Default) return;
            
            ChangeState(CookieState.Crushing);
            StartCoroutine(DoCookieMovement(ownUnit.data.targetMainRig.transform.position + Vector3.up * crushOffset, crushObject, crushCurve, crushEndDelay, true));
        }
        
        public void Push()
        {
            if (!ownUnit || !ownUnit.data || !ownUnit.data.targetMainRig || currentState != CookieState.Default) return;
            
            ChangeState(CookieState.Pushing);
            StartCoroutine(DoCookieMovement(ownUnit.data.mainRig.position + ownUnit.data.forwardGroundNormal * pushOffset, pushObject, pushCurve, pushEndDelay));
        }

        private IEnumerator DoCookieMovement(Vector3 pointToLerpTo, GameObject objectToSpawn, AnimationCurve curve, float endDelay, bool pointUp = false)
        {
            if (!ownUnit || !ownUnit.data || !ownUnit.data.targetMainRig) yield break;

            var spawnedCookie = Instantiate(objectToSpawn, transform.TransformPoint(modelOffset), transform.rotation);
            var startPos = spawnedCookie.transform.position;
            var startRot = spawnedCookie.transform.rotation;
            var t = 0f;
            var endTime = curve.keys[curve.keys.Length - 1].time;
            while (t < endTime)
            {
                spawnedCookie.transform.position = Vector3.Lerp(startPos, pointToLerpTo, curve.Evaluate(t));
                spawnedCookie.transform.rotation = Quaternion.Lerp(startRot, pointUp ? Quaternion.LookRotation(Vector3.up) : Quaternion.LookRotation(pointToLerpTo - startPos), curve.Evaluate(t));
                t += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(endDelay);

            ChangeState();
        }

        private Unit ownUnit;
        
        public enum CookieState
        {
            Default,
            Crumbled,
            Pushing,
            Crushing
        }

        public CookieState currentState;
        
        public Vector3 modelOffset;
        
        public List<ConditionalEvent> movesToStun = new List<ConditionalEvent>();
        
        [Header("Default Settings")]
        
        public UnityEvent defaultEvent = new UnityEvent();
        
        [Header("Crumble Settings")]
        
        public UnityEvent crumbleEvent = new UnityEvent();

        public GameObject crumbleObject;

        public float crumbleTime;

        public float maxHealth = 500f;
        private float health;

        [Header("Push Settings")] 
        
        public UnityEvent pushEvent = new UnityEvent();
        public GameObject pushObject;

        public AnimationCurve pushCurve;
        public float pushOffset = 5f;
        public float pushEndDelay;
        
        [Header("Crush Settings")] 
        
        public UnityEvent crushEvent = new UnityEvent();
        public GameObject crushObject;

        public AnimationCurve crushCurve;
        public float crushOffset = 5f;
        public float crushEndDelay;
    }
}