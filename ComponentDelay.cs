using UnityEngine;
using System.Collections;

public class ComponentDelay : MonoBehaviour {

	public MonoBehaviour component;
	public float SecondsDelay;

	void Start() {
		if (SecondsDelay == 0) {
			Destroy (this); // 0 seconds = no delay
		}

		component.enabled = false;
		Invoke ("activate", SecondsDelay);
	}

	void activate() {
		component.enabled = true;
		Destroy (this);
	}
}
