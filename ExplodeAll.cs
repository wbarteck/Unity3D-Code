using UnityEngine;
using System.Collections;

public class ExplodeAll : MonoBehaviour {

	public ParticleSystem baseSystem;
	private ParticleSystem[] children;

	// Use this for initialization
	void Start () {
		if (baseSystem == null)
			baseSystem = GetComponent<ParticleSystem> ();
		if (baseSystem == null) {
			Debug.LogError ("No particle system attached: " + gameObject.name);
			this.enabled = false;
		}

		children = GetComponentsInChildren<ParticleSystem> ();

        ParticleSystem.MainModule m = baseSystem.main;
		m.playOnAwake = false;
		m.loop = false;
		baseSystem.Stop ();
		foreach (ParticleSystem sys in children) {
            ParticleSystem.MainModule m2 = baseSystem.main;
            m2.playOnAwake = false;
			m2.loop = false;
			sys.Stop ();
		}
	}

	public void Explode() {
		baseSystem.Play ();
		foreach (ParticleSystem sys in children) {
			sys.Play ();
		}
	}
}
