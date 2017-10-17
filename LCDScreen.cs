using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LCDScreen : MonoBehaviour {

	public DisplayText[] messages;

	public enum displayType {
		Fit,
		Scrolling,
		Flashing
	}
	public displayType type;
	public int maxCharacers = 9;		// Max characters in single line "Scrolling" display
	public float messageSpeed = 5f;
	public float flashSpeed = 2f;

	public Text textBox;

	private string message;
	private float t; 
	private float reset;

	// Use this for initialization
	void Start () {
		foreach (DisplayText dt in messages) {
			dt.message = fixString (dt.message);
		}

		if (messages.Length > 0)
			message = messages [0].message;
		else
			message = "._.";


	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime * messageSpeed;
	
		if (type == displayType.Fit) {
			textBox.text = message.Trim();
		}

		if (type == displayType.Scrolling) {
			t = t % (message.Length - 0);
			int index = (int)Mathf.Floor (t);
			int length = Mathf.Min(maxCharacers, message.Length - index);
			string str = message.Substring (index, length);
			textBox.text = str;
		}

		if (type == displayType.Flashing) {
			t = t % (flashSpeed *3); // restart cycle in 3x flash speed
			bool b = t < (flashSpeed * 2f); // on 2/3 of time, off 1/3 of time
			if (b) {
				textBox.text = message.Trim();
			} else {
				textBox.text = "";
			}
		}
	}

	public string fixString(string original) {
		original.Trim ();
		string a = "";
		for(int i = 0; i < maxCharacers; i++) {
			a += " ";
		}
		a += original;
		for(int i = 0; i < maxCharacers; i++) {
			a += " ";
		}
		return a;
	}

	public void SetMessage(string str) {
		t = 0;

		//TODO maybe play a sound or animation
		message = fixString (str);
	}
}
[System.Serializable]
public class DisplayText {
	public string message;
}