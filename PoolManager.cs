using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	public static PoolManager instance;

	public ObjectPool[] projectiles;

	public ObjectPool[] utilities;

	public ObjectPool[] effects;

	void Start () {
        if (instance == null)
		    instance = this;

		setup ();
	}

	void setup() {
		// Setup Projectiles Pools
		projectiles = new ObjectPool[Armory.armory.projectiles.Count];
		for (int i = 0; i  < projectiles.Length; i++) {
			ObjectPool op = new ObjectPool ();
			op.prefab = Armory.armory.projectiles [i];
			op.startSize = 5;
			op.component = op.prefab.GetComponent<Bullet>();

			projectiles [i] = op;
			op.Setup (transform);
		}

		// Setup Utilities Pools
		utilities = new ObjectPool[Armory.armory.utilities.Count];
		for (int i = 0; i  < utilities.Length; i++) {
			ObjectPool op = new ObjectPool ();
			op.prefab = Armory.armory.utilities [i];
			op.startSize = 5;
			op.component = op.prefab.GetComponent<UtilitySpawn> ();

			utilities [i] = op;
			op.Setup (transform);
		}

		// particle effects pool
		StartCoroutine(getEffects());
	}

	public IEnumerator getEffects() {
		
		do {
			if (ParticleEffectLibrary.instance != null)
				effects = new ObjectPool[ParticleEffectLibrary.instance.effects.Length];
			yield return null;
		} while (effects.Length == 0);

		for (int i = 0; i < ParticleEffectLibrary.instance.effects.Length; i++) {
			ObjectPool op = new ObjectPool ();
			op.prefab = ParticleEffectLibrary.instance.effects [i].particlePrefab.gameObject;
			op.startSize = 5;
			op.component = op.prefab.GetComponent<ComplexParticleSystem> ();
			
			effects [i] = op;
			op.Setup (transform);
		}
	}

    public void DisableAll()
    {
        foreach(ObjectPool op in projectiles)
        {
            foreach(PoolEntry entry in op.objects)
            {
                entry.gameObject.SetActive(false);
            }
        }
    }
}
[System.Serializable]
public class ObjectPool {
	public GameObject prefab;
	public int startSize = 1;
	public MonoBehaviour component;
	public List<PoolEntry> objects;
	private GameObject parent;

	// TODO custom add
	public void Setup(Transform p) {
		objects = new List<PoolEntry> ();

		parent = new GameObject ();
		parent.transform.SetParent (p);
		parent.name = component.name + " Pool";

		for (int i = 0; i < startSize; i++) {
			GrowPool ();
		}
	}

	private PoolEntry GrowPool() {
		PoolEntry entry = new PoolEntry ();
		GameObject o = GameObject.Instantiate (prefab) as GameObject;
		o.name = component.name.ToString ();
		o.transform.SetParent (parent.transform);
		o.SetActive (false);
		entry.gameObject = o;
		entry.behavior = o.GetComponent (component.GetType());
		objects.Add (entry);
		return entry;
	}

	public PoolEntry getUnused() {
		foreach (PoolEntry p in objects) {
			if (!p.gameObject.activeInHierarchy)
				return p;
		}
		//no unused, grow pool
		return GrowPool();
	}

	public void ReturnToPool(Object o) {
		foreach (PoolEntry p in objects) {
			if (p.gameObject == (GameObject)o ||
			    p.behavior == (Component)o) {
				p.gameObject.transform.position = Vector3.zero;
				p.gameObject.transform.rotation = Quaternion.identity;
				p.gameObject.SetActive (false);
				return;
			}
		}
	}
}
[System.Serializable]
public class PoolEntry {
	public GameObject gameObject;
	public Component behavior;
}