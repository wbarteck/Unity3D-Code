using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TestExplosionForce : MonoBehaviour {

	public float speed = 5f;
	public float radius = 5f;
	public float force = 1000f;

	public UnityEvent explode;

	private Vector3 fwd;
	private Vector3 right;

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, radius);
	}

	void Explode() {
		Collider[] cols = Physics.OverlapSphere (transform.position, radius);
		foreach (Collider c in cols) {
			PhysicsBodyManager r = c.GetComponent<PhysicsBodyManager> ();
			if (r != null) {
				r.WakeNBake ();
				r.rigidBody.AddExplosionForce (force, transform.position, radius);
			}
		}
		Debug.Log ("EXPLODE");

		explode.Invoke ();
	}
	// Update is called once per frame
	void Update () {
		fwd = Camera.main.transform.forward;
		fwd.y = 0;
		fwd.Normalize ();
		right = Camera.main.transform.right;
		right.y = 0;
		right.Normalize ();


		Vector3 pos = transform.position;
		pos += fwd * Input.GetAxis ("Vertical") * speed * Time.deltaTime;
		pos += right * Input.GetAxis ("Horizontal") * speed * Time.deltaTime;
		if (Input.GetKey(KeyCode.LeftShift))
			pos.y +=  10f * Time.deltaTime;
		if (Input.GetKey(KeyCode.LeftControl))
			pos.y -=  10f * Time.deltaTime;
		transform.position = pos;

		if (Input.GetKeyDown (KeyCode.Space))
			Explode ();
	}
}
