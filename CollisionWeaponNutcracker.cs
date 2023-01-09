using System;
using System.Collections.Generic;
using Landfall.MonoBatch;
using Landfall.TABS;
using UnityEngine;
using UnityEngine.Events;

namespace Holiday
{
	public class CollisionWeaponNutcracker : BatchedMonobehaviour, GameObjectPooling.IPoolable
	{
		public enum CallEffectsOn
		{
			Rigidbodies,
			Ground,
			All
		}
	
		private MeleeWeaponMultiplierPoint multiplierPoint;

		public bool canDealDamage;
	
		public float damage = 80f;
	
		public float selfDamageMultiplier;
	
		public float impactMultiplier = 1f;
	
		public float enemyMassPow = 1f;
	
		public float minVelocity;
	
		public float screenShakeMultiplier = 1f;
	
		public float onImpactForce;
	
		public float massCap = 5f;
	
		public bool ignoreTeamMates;
	
		public float teamDamage = 0.1f;
	
		public bool staticDamageValue;
	
		public bool onlyOncePerData;
	
		public bool hitFasterAfterDealDamage = true;
	
		public bool useHitDirection;
	
		public bool playSoundWhenHitNonRigidbodies = true;
	
		private List<DataHandler> hitDatas = new List<DataHandler>();
	
		private Holdable holdable;
	
		private DataHandler connectedData;
	
		[HideInInspector]
		public List<Rigidbody> protectedRigs = new List<Rigidbody>();
	
		private CollisionWeaponEffect[] meleeWeaponEffects;
	
		private Weapon weapon;
	
		private Counter counter;
	
		private TeamHolder teamHolder;
	
		public UnityEvent dealDamageEvent;
	
		private CollisionSound collisionSound;
	
		public Damagable lastHitHealth;
	
		public float cooldown;
	
		private float sinceLastDamage;
	
		[HideInInspector]
		public bool onlyCollideWithRigs;
	
		private ParticlePlayer m_particlePlayer;
	
		private Level myLevel;
	
		private Unit ownUnit;
	
		public CallEffectsOn callEffectsOn;
	
		private Rigidbody rig;
	
		private bool hitTeamMate;
	
		private Action<Collision, float> CollisionAction;
	
		private Action<Collision, float, Vector3> DealDamageAction;
	
		public Action ReleaseSelf { get; set; }
	
		private void Awake()
		{
			m_particlePlayer = ServiceLocator.GetService<ParticlePlayer>();
		}
	
		protected override void Start()
		{
			base.Start();
			collisionSound = GetComponent<CollisionSound>();
			rig = GetComponent<Rigidbody>();
			holdable = GetComponent<Holdable>();
			counter = GetComponent<Counter>();
			teamHolder = GetComponent<TeamHolder>();
			weapon = GetComponent<Weapon>();
			meleeWeaponEffects = GetComponents<CollisionWeaponEffect>();
			multiplierPoint = GetComponentInChildren<MeleeWeaponMultiplierPoint>();
			if ((bool)weapon)
			{
				damage *= weapon.levelMultiplier;
			}
			myLevel = GetComponent<Level>();
			if ((bool)myLevel)
			{
				onImpactForce *= Mathf.Pow(myLevel.level, 1.5f);
				massCap *= Mathf.Pow(myLevel.level, 1.5f);
			}
		}
	
		public override void BatchedUpdate()
		{
			if (cooldown != 0f)
			{
				sinceLastDamage += Time.deltaTime;
			}
		}
	
		private void OnCollisionEnter(Collision collision)
		{
			if (onlyCollideWithRigs && !collision.rigidbody)
			{
				return;
			}
			CollisionAction?.Invoke(collision, 0f);
			if (!canDealDamage)
			{
				return;
			}
			float num = 0f;
			if ((bool)rig)
			{
				if ((bool)collision.rigidbody)
				{
					_ = collision.rigidbody.mass;
					_ = enemyMassPow;
					_ = 1f;
					num = collision.impulse.magnitude / (rig.mass + 10f) * 0.3f;
				}
				else
				{
					num = collision.impulse.magnitude / rig.mass * 0.3f;
				}
			}
			num *= impactMultiplier;
			num = Mathf.Clamp(num, 0f, 2f);
			if (num < 1f)
			{
				return;
			}
			if (!collision.rigidbody)
			{
				DoScreenShake(num, collision);
				if (callEffectsOn == CallEffectsOn.All || callEffectsOn == CallEffectsOn.Ground)
				{
					DoCollisionEffects(collision.transform, collision);
				}
				if ((bool)collisionSound && playSoundWhenHitNonRigidbodies)
				{
					collisionSound.DoEffect(collision.transform, collision, num);
				}
			}
			if (minVelocity != 0f && (bool)rig && rig.velocity.magnitude < minVelocity)
			{
				return;
			}
			if (!connectedData && (bool)holdable && holdable.held)
			{
				connectedData = holdable.holderData;
			}
			if (collision.transform.root == base.transform.root || ((bool)connectedData && connectedData.transform.root == collision.transform.root) || !collision.rigidbody || protectedRigs.Contains(collision.rigidbody) || sinceLastDamage < cooldown)
			{
				return;
			}
			sinceLastDamage = 0f;
			DataHandler componentInParent = collision.rigidbody.GetComponentInParent<DataHandler>();
			Damagable componentInParent2 = collision.rigidbody.GetComponentInParent<Damagable>();
			if ((bool)componentInParent2)
			{
				if ((bool)componentInParent && onlyOncePerData)
				{
					if (hitDatas.Contains(componentInParent))
					{
						return;
					}
					hitDatas.Add(componentInParent);
				}
				if ((bool)weapon && lastHitHealth == componentInParent2)
				{
					return;
				}
				lastHitHealth = componentInParent2;
				if ((bool)collisionSound)
				{
					collisionSound.DoEffect(collision.transform, collision, num);
				}
				Unit unit = (connectedData ? connectedData.GetComponentInParent<Unit>() : GetComponentInParent<Unit>());
				Unit unit2 = null;
				if ((bool)componentInParent)
				{
					unit2 = componentInParent.GetComponentInParent<Unit>();
				}
				if (!holdable && (bool)unit && unit.data.Dead)
				{
					UnityEngine.Object.Destroy(this);
					return;
				}
				float num2 = num;
				if (staticDamageValue)
				{
					num2 = 1f;
				}
				if ((bool)multiplierPoint && Vector3.Distance(collision.contacts[0].point, multiplierPoint.transform.position) < multiplierPoint.range)
				{
					num2 *= multiplierPoint.multiplier;
				}
				if ((bool)unit2 && (bool)teamHolder && unit2.Team == teamHolder.team)
				{
					if (((bool)myLevel && myLevel.ignoreTeam) || ((bool)counter && !(counter.counter > 0.2f)))
					{
						return;
					}
					num2 *= 0.1f;
				}
				if ((bool)unit && (bool)unit2 && unit.Team == unit2.Team)
				{
					if (((bool)myLevel && myLevel.ignoreTeam) || ignoreTeamMates)
					{
						return;
					}
					num2 *= teamDamage;
				}
				Vector3 vector = base.transform.forward;
				if (useHitDirection)
				{
					vector = collision.transform.position - base.transform.position;
				}
				if ((bool)componentInParent)
				{
					WilhelmPhysicsFunctions.AddForceWithMinWeight(componentInParent.mainRig, (staticDamageValue ? 5f : Mathf.Sqrt(num * 50f)) * vector * onImpactForce, ForceMode.Impulse, massCap);
					WilhelmPhysicsFunctions.AddForceWithMinWeight(collision.rigidbody, (staticDamageValue ? 5f : Mathf.Sqrt(num * 50f)) * vector * onImpactForce, ForceMode.Impulse, massCap);
				}
				if ((bool)weapon && collision.rigidbody.mass < weapon.rigidbody.mass)
				{
					collision.rigidbody.velocity *= 0.6f;
					if ((bool)componentInParent)
					{
						componentInParent.mainRig.velocity *= 0.6f;
					}
				}
				if (!ownUnit)
				{
					if ((bool)connectedData && (bool)connectedData.unit)
					{
						ownUnit = connectedData.unit;
					}
					else if ((bool)teamHolder && (bool)teamHolder.spawner)
					{
						ownUnit = teamHolder.spawner.GetComponentInParent<Unit>();
					}
				}
				DealDamageAction?.Invoke(collision, damage * num2, vector);
				lastHitHealth.TakeDamage(damage * num2, vector, ownUnit);
				_ = damage;
				if (selfDamageMultiplier != 0f && (bool)unit)
				{
					unit.data.healthHandler.TakeDamage(damage * num2 * selfDamageMultiplier, vector, null);
				}
				if (dealDamageEvent != null)
				{
					dealDamageEvent.Invoke();
				}
				if ((bool)weapon && hitFasterAfterDealDamage)
				{
					weapon.internalCounter += UnityEngine.Random.Range(0f, weapon.internalCooldown * 0.5f);
				}
				if ((bool)componentInParent && (callEffectsOn == CallEffectsOn.All || callEffectsOn == CallEffectsOn.Rigidbodies))
				{
					DoCollisionEffects(componentInParent.mainRig.transform, collision);
				}
			}
			else
			{
				if ((bool)collisionSound)
				{
					collisionSound.DoEffect(collision.transform, collision, num);
				}
				if ((bool)weapon)
				{
					WeaponCollision(collision, num);
				}
			}
			DoScreenShake(num, collision);
		}
	
		private void DoCollisionEffects(Transform targetTransform, Collision collision)
		{
			if (meleeWeaponEffects != null && meleeWeaponEffects.Length != 0)
			{
				for (int i = 0; i < meleeWeaponEffects.Length; i++)
				{
					meleeWeaponEffects[i].DoEffect(targetTransform, collision);
				}
			}
		}
	
		private void DoScreenShake(float impact, Collision collision, float m = 1f)
		{
			if (ScreenShake.Instance != null)
			{
				ScreenShake.Instance.AddForce(base.transform.forward * Mathf.Sqrt(impact * 0.5f) * 0.5f * screenShakeMultiplier, collision.contacts[0].point);
			}
		}
	
		private void WeaponCollision(Collision collision, float impact)
		{
		}
	
		public void AddCollisionAction(Action<Collision, float> action)
		{
			CollisionAction = (Action<Collision, float>)Delegate.Combine(CollisionAction, action);
		}
	
		public void AddDealDamageAction(Action<Collision, float, Vector3> action)
		{
			DealDamageAction = (Action<Collision, float, Vector3>)Delegate.Combine(DealDamageAction, action);
		}
	
		public void Initialize()
		{
		}
	
		public void Reset()
		{
		}
	
		public void Release()
		{
			hitDatas.Clear();
		}

		public void SetCanDealDamage(bool value)
		{
			canDealDamage = value;
		}

		public bool IsManagedByPool { get; set; }
	}
}