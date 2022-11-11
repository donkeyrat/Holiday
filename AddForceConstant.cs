using System;
using UnityEngine;
using Landfall.TABS;
using System.Collections;

public class AddForceConstant : MonoBehaviour, GameObjectPooling.IPoolable
{
	public Vector3 force;

	public Vector3 worldForce;

	public Vector3 worldForce_Random;

	public Vector3 worldTorque;

	public Vector3 worldTorque_Random;

	private Rigidbody[] rig;

	private bool m_initialized;

	public bool playOnStart = true;

	public Action ReleaseSelf
	{
		get;
		set;
	}

	private void Start()
	{
		Initialize();
	}

	public void Initialize()
    {
		if (!m_initialized)
		{
			m_initialized = true;
			var u = GetComponent<UnitSpawner>().Spawn();
			u.transform.position = transform.position;
			u.transform.rotation = transform.rotation;
			rig = u.GetComponentsInChildren<Rigidbody>();

			for (int i = 0; i < rig.Length; i++)
			{
				rig[i].AddForce(base.transform.TransformDirection(GetComponent<AddForce>().force), ForceMode.VelocityChange);
				Vector3 b = new Vector3(UnityEngine.Random.Range(0f - worldForce_Random.x, worldForce_Random.x), UnityEngine.Random.Range(0f - worldForce_Random.y, worldForce_Random.y), UnityEngine.Random.Range(0f - worldForce_Random.z, worldForce_Random.z));
				rig[i].AddForce(worldForce + b, ForceMode.VelocityChange);
				Vector3 b2 = new Vector3(UnityEngine.Random.Range(0f - worldTorque_Random.x, worldTorque_Random.x), UnityEngine.Random.Range(0f - worldTorque_Random.y, worldTorque_Random.y), UnityEngine.Random.Range(0f - worldTorque_Random.z, worldTorque_Random.z));
				rig[i].AddTorque(worldTorque + b2, ForceMode.VelocityChange);
			}
		}
	}

	public void Reset()
	{
		m_initialized = false;
		for (int i = 0; i < rig.Length; i++)
		{
			rig[i].velocity = Vector3.zero;
		}
	}

	public void Release()
    {
    }

	public bool IsManagedByPool { get; set; }
}
