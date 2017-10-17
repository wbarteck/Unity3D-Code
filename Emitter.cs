using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour {

    public enum SpawnType
    {
        Trigger,
        PerSecond
    }

    [Header("Object to Spawn")]
    public GameObject obj;

    [Header("If Has Rigidbody")]
    public bool HasRB = false;
    public Vector3 initialVelocity;
    public Vector3 angularVelocity;
    public bool moveAnyway = false;

    [Header("Timing")]
    public SpawnType spawnType;
    public float maxCount = Mathf.Infinity;
    private float spawned = 0f;
    public float maxAlive = 1f;
    public float spawnTime = 1f;
    public float lifetime = 5f;

    public List<GameObject> spawns = new List<GameObject>();

	public void Spawn()
    {
        if (spawns.Count >= maxAlive || spawned >= maxCount) //Dont do anything if already at max
            return;

        GameObject spawn = Instantiate(obj) as GameObject;
        spawn.transform.SetPositionAndRotation(transform.position, transform.rotation);
        spawn.name = obj.name + " Spawn";

        if (HasRB)
        {
            Rigidbody rb = spawn.GetComponent<Rigidbody>();
            if (rb == null) //does not actually have rb 
            {
                Debug.LogWarning("Emitter spawning " + obj.name + " error: obj does not have rb");
                HasRB = false;
            } else
            {
                rb.velocity = initialVelocity;
                rb.angularVelocity = angularVelocity;
            }
        } else if (moveAnyway)
        {
            StartCoroutine(fakeMove(spawn));
        }
        spawned++;
        spawns.Add(spawn);
        StartCoroutine(liveNLetDie(spawn));
    }
    
    IEnumerator fakeMove(GameObject o)
    {
        while (o.activeInHierarchy)
        {
            o.transform.Translate(initialVelocity * Time.deltaTime);
            o.transform.Rotate(angularVelocity * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator liveNLetDie(GameObject o)
    {
        float time = 0f;
        while (time < lifetime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        spawns.Remove(o);
        Destroy(o);
    }
	
	void Update () {
		if (spawnType == SpawnType.PerSecond)
        {
            if (!IsInvoking("Spawn"))
                Invoke("Spawn", spawnTime);
        }
	}
}
