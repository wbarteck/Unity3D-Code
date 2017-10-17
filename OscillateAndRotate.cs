using UnityEngine;
using System.Collections;

public class OscillateAndRotate : MonoBehaviour {

	public float period = 2f;
	public float amplitude = .5f;
	public float rotation = 90f;

	private Vector3 rest;

	private float t = 0f;

	// Use this for initialization
	void Start () {
		rest = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		t = t % (period * 2 );

		transform.localPosition = rest * amplitude * Mathf.Sin (t * Mathf.PI / period);
		Vector3 euler = transform.localEulerAngles;
		euler.y += rotation * Time.deltaTime;
		transform.localEulerAngles = euler;
	}
}
