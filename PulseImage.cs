using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class PulseImage : MonoBehaviour {

	public float t = 1;
	float time;

	private Graphic obj;

	// Use this for initialization
	void Start () {
		obj = GetComponent<Graphic> ();


	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		Color c = obj.color;
		c.a = .5f * Mathf.Sin (time / t) + .5f;
		obj.color = c;
	}
}
