using UnityEngine;
using System.Collections;

public class SleepTest : MonoBehaviour {

	public Rigidbody rb;

	private Material instanceMaterial = null;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
		if (rb == null) {
			Debug.LogError ("Trying to test rigidbod on object without RB: " + gameObject.name);
			Destroy (this);
		}

		MeshRenderer rend = GetComponent<MeshRenderer> ();
		instanceMaterial = rend.material = rend.material; //create an automatic copy
	}

	void Update () {
		if (rb.IsSleeping ())
			instanceMaterial.color = Color.red;
		else
			instanceMaterial.color = Color.green;
	}
}
