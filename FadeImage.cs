using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class FadeImage : MonoBehaviour {

	public bool FadeOnAwake;
	public bool fadeIn;

	public float fadeTime;

	private Image img;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();

		if (FadeOnAwake) {
			if (fadeIn) {
				Color a = img.color;
				a.a = 0f;
				img.color = a;
				StartCoroutine (Fade (1f, fadeTime));
			} else {
				Color a = img.color;
				a.a = 1f;
				img.color = a;
				StartCoroutine (Fade (0f, fadeTime));
			}

		}
	}

	IEnumerator Fade(float alpha, float time) {
		Color original = img.color;
		Color copy = img.color;
		copy.a = alpha;
		for (float t = 0; t < time; t += Time.deltaTime) {
			Color result = Color.Lerp (original, copy, t/ time);
			img.color = result;
			yield return null;
		}
	}
}
